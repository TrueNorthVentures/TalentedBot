using System;
using System.Net;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using SmbiBotApp.Services;
using Microsoft.Bot.Builder.Luis.Models;
using SmbiBotApp.Model;
using Microsoft.Bot.Builder.Dialogs.Internals;
using System.Text;
using Microsoft.Bot.Builder.Dialogs;
using System.Reflection;
using Autofac;
using System.Collections.Generic;
using System.Windows.Input;
using System.Web.UI.WebControls;
using Facebook;
using System.Threading;
using Humanizer;
using System.Xml.Serialization;
using System.Web.Script.Serialization;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Xml;
using MongoDB.Bson.Serialization;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Web.Razor;
using SmbiBotApp.Controllers;

namespace SmbiBotApp
{

    //[Serializable]
    //public class DisplayDialog : IDialog<object>
    //{
    //    private ResumeAfter<bool> play;

    //    public  async Task StartAsync(IDialogContext context)
    //    {
    //       await context.PostAsync("Are you sure you want to save");
    //       // PromptDialog.Confirm(context,play,"create the profile");
    //      //  await context.Wait(SendAsync);
    //          context.Wait(MessageReceivedAsync);       
    //    }

    //    public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
    //    {
    //        await context.PostAsync("Not correct. Guess again.");
    //        PromptDialog.Confirm(context,play,"Yes I did it");

    //        // context.Wait(MessageReceivedAsync);          
    //    }
    //}

    [Serializable]
    public class RootDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            // Root dialog initiates and waits for the next message from the user. 
            // When a message arrives, call MessageReceivedAsync.
            context.Wait(this.MessageReceivedAsync);
        }

        public virtual async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result; // We've got a message!
            await context.PostAsync($"welcome back to the bot");
            //if (message.Text.ToLower().Contains("order"))
            //{
            //    // User said 'order', so invoke the New Order Dialog and wait for it to finish.
            //    // Then, call ResumeAfterNewOrderDialog.
            //  //  await context.Forward(new NewOrderDialog(), this.ResumeAfterNewOrderDialog, message, CancellationToken.None);
            //}
            // User typed something else; for simplicity, ignore this input and wait for the next message.
            context.Wait(this.MessageReceivedAsync);
        }

        private async Task ResumeAfterNewOrderDialog(IDialogContext context, IAwaitable<string> result)
        {
            // Store the value that NewOrderDialog returned. 
            // (At this point, new order dialog has finished and returned some value to use within the root dialog.)
            var resultFromNewOrder = await result;

            await context.PostAsync($"New order dialog just told me this: {resultFromNewOrder}");

            // Again, wait for the next message from the user.
            context.Wait(this.MessageReceivedAsync);
        }
    }


    public partial class DerivedActivity : Activity
    {
       
        public DerivedActivity(Activity reply):base() 
        {
            this.Text = reply.Text;
            this.Attachments = new List<Attachment>(reply.Attachments);
            this.AttachmentLayout = reply.AttachmentLayout;         
        }

    }

    [BotAuthentication]
    public class MessagesController : ApiController
    {
       
        HttpClient client = new HttpClient();//1
        public static string userid = null;//2
        public static string acctoken = null;//4
        //public static readonly Uri FacebookOauthCallback = new Uri("http://smbibotapp20170804124326.azurewebsites.net/api/OAuthCallback");//3//5
        //public static readonly Uri FacebookOauthCallback = new Uri("http://localhost:3975/api/OAuthCallback");//3//5
        public static List<string> col = new List<string>();
        public static List<string> data = new List<string>();
        public static List<string> avoid = new List<string>();
        static string[] profile = new string[7];
        public static string[] missing = new string[7];
        static int missed = 0;
        static int already = 0;
        static int count = 0;
        static int flag = 0;
        static int counter = 1;
        static int callback = 0;
        static int des_pro = 0;
        static int update_info = 0;
        static string id = null;
        static string occupation = null;
        static string entity = null;
        static int pro_count = 1;
        static bool wait = true;
        static int ques_count = 0;
        static int score = 0;
        static int flagi = 0;
        public static int repeat = 1;
        List<string> ques = new List<string>();
        List<string> options = new List<string>();
        public static Testing testsretrieved = null;
        public static Record[] display = null;
        // public  IDialogContext context;
        // public ResumeAfter<string> AfterUserInputSymbol;

        //public enum decision
        //{
        //    firstName = 1,
        //    lastName = 2,
        //    myAge = 3,
        //    Gender = 4,
        //    currentAddress = 5,
        //    emailAddress = 6,
        //    university = 7,
        //    graduate = 8,
        //    degree = 9,
        //    projects = 10,
        //    title = 11,
        //    description = 12,
        //    viewProfile = 13,
        //};

        public enum decision
        {
            fullName = 1,
            currentAddress = 2,
            myAge = 3,
            emailAddress = 4,
            qualification = 5,
            graduate = 6,
            degree = 7,
            university = 8,
            designation = 9,
            experience = 10,
            firm = 11,
            //projectInfo = 12
            title = 12,
            description = 13,
            technologies = 14,
            completionTime = 15

        };



    public enum user_profile
        {
            id = 0,
            first_name = 1,
            last_name = 2,
            birthday = 3,
            gender = 4,
            location = 5,
            email = 6,

        };


        private void asked()
        {
            col.Add("Before I get you matched with positions best suited for you. I will need to know a little about you");//// 0
            col.Add("What name do you prefer being called by ?");//// 1
            col.Add("What are you looking for a role in ?");//// 2
            col.Add("What sort of role are you interested in ?");//// 3
            col.Add("What sort of position are you looking for ?");//// 4
            col.Add("What city do you currently reside in ?");//// 5
            col.Add("When is your birthday ?");//// 6
            col.Add("Just in case I lose you or need to get in touch. What would be the best email to reach you at ?");//// 7
            col.Add("UserName now that I have a grasp of what your looking for lets play a word game to unlock your personality and get insights into companies that would be right for you");//// 8
            col.Add("Below are some of the types of companies that are looking to hire talent with same goals as you");//// 9
            col.Add("Now let's get your profile together so we can get on finding the best fit for you");//// 10
            col.Add("Do you have any formal education ?");//// 11
            col.Add("What is your Highest Achieved Educational Qualification ?");//// 12
            col.Add("What did did you study before ?");//// 13
            col.Add("When did you graduate ?");//// 14
            col.Add("What did you study ?");//// 15
            col.Add("Where did you study from ?");///// 16
            col.Add("Did You study anything else ?");//// 17
            col.Add("Now that I know about your schooling lets talk about your experience working in positions and on projects ?");//// 18
            col.Add("Not everything is to be learned in school. Lets talk about all the work you've done in positions or in projects that have helped shape your skills");//// 19
            col.Add("What would you want to talk about first ?");//// 20
            col.Add("Lets get started on learning about this position");//// 21
            col.Add("What was the position you held ?");//// 22
            col.Add("How long did you hold this position?");//// 23
            col.Add("What company was this position with ?");//// 24
            col.Add("Mind describing what you did and achieved in the position");//// 25
            col.Add("What was the most recent project you worked on called ?");//// 26
            col.Add("What was the project you about ?");//// 27
            col.Add("What technologies did you use ?");//// 28
            col.Add("How long did the project last ?");//// 29

            col.Add("my name is");//// 30
            col.Add("I currently live in");//// 31
            col.Add("my age is");//// 32
            col.Add("my email address is");//// 33
            col.Add("the level of education that i have been achieved is");//// 34
            col.Add("I graduated in");//// 35
            col.Add("I study");//// 36
            col.Add("I attend");//// 37
            col.Add("my designation is");//// 38
            col.Add("I have experience of");//// 39
            col.Add("the firm is");//// 40
           

            col.Add("Before I get matching you to the perfect job. I'll have to first setup your profile.");
            col.Add("This is the information we were able to pull from facebook could you confirm if you would like me to use this information ?");
            col.Add("What is your firstname ?");
            col.Add("What is your last name ?");
            col.Add("How old are you ?");
            col.Add("Are you a guy or a girl ?");
            col.Add("Where are you currently living (city)?");
            col.Add("what is your email address ?");
            col.Add("Now that we're acquainted I would like to learn a little about your education and skills.");
            col.Add("What university did you attend ?");//9
            col.Add("When did you graduate ?");//10
            col.Add("What did you study ?");//11
            col.Add("Is this the highest level of education you attainted ?");//12
            col.Add("What sort of company are you interested in for investment ?");//13
            col.Add("What sort of designer are you ?");//14
            col.Add("How many projects or products you have worked on ?");//15
            col.Add("what is the title of your");//16
            col.Add("Briefly describe your"); //17
            col.Add("Rank your experience / level of using design softwares:Sketch:Photoshop:Illustrator:indesign:");
            col.Add("What web development languages are you familiar with : (select all those that apply)");//19
            col.Add("Describe the project or role you were most proud of ? What sort did you do in your capcity and what were the outcomes / achievements ? ");
            col.Add("Is there any other project, product , or work experience. You would like to share with us ?");
            col.Add("what sort of marketer are you ?");//22
            col.Add("my first name is");//13//23
            col.Add("my last name is");//14//24
            col.Add("my age is");//15//25
            col.Add("my gender is");//16//26
            col.Add("I currently live in");//17//26//27
            col.Add("my email address is");//28
            col.Add("I attend");//29
            col.Add("I graduated in");//30
            col.Add("I study");//31
            col.Add("Are you done");//32
            col.Add("What sort of role are you looking for ?");//33
            col.Add("Your profile has been successfully created");//34
            col.Add(" I'm maaz a chabot developed by folks at PeopleHome to help you find the job right for you !");//35
            col.Add("Project that i have been done so far are");//36
            col.Add("The title of my project is");//37
            col.Add("The description of my project is");//38
            col.Add("That's good now your basic profile has been set.If you want to update your profile information let us know ?");//39
            col.Add("Now we are going to take some tests to evaluate you and to build your professional profile");//40
            col.Add("Which technology you are intertested ?");//41
            col.Add("Which type of test you would like to give ?"); //42
            col.Add("Now let start the test here is your first question");//43
            col.Add("Do you want to give any other test ?");//44
            col.Add("Ok thanks for visiting our bot and share your experience");//45
            col.Add("You have been already given that test");//46
            col.Add("Which information would you like to update ?");//47
            col.Add("What sort of developer are you ?");//48
            col.Add("you will need to complete the following modules at your own pace");//49
            col.Add("You will be tested on the skills and tools that are utilized as a front,backend,fullstack developer");//50
            col.Add("Demonstrate your core knowledge needed to program,how to write and execute applications");//51
            col.Add("Show your understanding and ability to utilize the languages powering the frontend of the web(HTML5,JavaScript,CSS)");//52
            col.Add("You will be tested on your understanding of both databases and their architecture whatever the programming language,operating system or application type be"); // 53
            col.Add("Show your understanding of the most commom backend languages that power processing and databases(PHP,Python,SQL)");// 54
            col.Add("Demonstrate your understanding of the most important security concerns when developing websites and what can be done to keep servers,software and data safe from harm");//55
            col.Add("Show your understanding of the workflow that makes it easier to build websites,track share project files and leverage code libraries");//56
            col.Add("Demonstrate your knowledge of the frontend and backend framework libraries to unlock more oppertunities");//57
            col.Add("Demonstrate your ability to apply design principles to your site to make it behave in ways that users want and expect");//58 
            col.Add("Demonstrate your ability to integrate accessibility into the design process to make your projects more accessible to all people");//59
            col.Add("Demonstrate your understanding of the concepts behind responsive design, covering concepts of density, fluid grids, images, as well as design strategies that guide the project");//60
            col.Add("Show your understanding and ability to utilize the most common frontend frameworks and tools (Bootstrap 3, React.js, Sass)"); //61
            col.Add("Demonstrate your skills in basics of Object-Oriented programming from abstraction and inheritance to your ability to use cases and create a conceptual modesl for your application");//62
            col.Add("Explain your understanding of Java SE, in which, counting a few, fancy mobile apps, desktop, web applications are built");//63
            col.Add("Show how you cater with common Java challenges that occur during the development process.");//64
            col.Add("Demonstrate your knowledge of the 7 object oriented design patters (singleton, observer, decorator, factor) that emblish your development skills.");//65
            col.Add("Demonstrate your ability to use Java to its fullest through platform and framework  agonistic code");//66
            col.Add("Demonstrate how well can you read and manage data from relational databases such as MySQL and SQL Server using the Java Database Connectivity (JDBC) API in applications programmed with Java");//67
            col.Add("You will be tested on online marketing techniques specifically on how to build a successful online marketing campaign for all digital, channels: search, video, social, email, and display");//68
            col.Add("Prove your understanding of the foundational concepts of search engine optimization from keyword planning, content optimization, link building, for ecommerce, local search, and mobile audiences");//69
            col.Add("You will be tested on your knowledge of using google analytics to measure website performance, traffic, advert performance, and social media activity");//70
            col.Add("Demonstrate your ability in developing, implementing, and measuring a successful content marketing strategy");//71
            col.Add("Demonstrate your knowledge of ensuring marketing campaigns keep up with pace of mobile, having your website and emails optimized for mobile visitors, launching SMS campaigns, advertising on mobile, and much more");//72
            col.Add("You will be tested on your knowledge of starting a lead generation program and converting prospects into loyal customers");//73
            col.Add("Demonstrate your understanding of growth hacking techniques that can quickly expand your customer base using low cost methods");//74
            col.Add("You will be tested on your abilities to integrate all the moving parts – email, social, media, search into a successful message of your brand");//75
            col.Add("Demonstrate your understanding of basic sales skills used to develop professionally and build more successful relationships");//76
            col.Add("Have your listening skills and behavior evaluated");//77
            col.Add("Demonstrate your ability to interact with others by being assertive");//78
            col.Add("Demonstrate your ability to bounce back from difficult situations");//79
            col.Add("Demonstrate your understanding of the essential steps that underlie ever sales process and their implementation into a workflow");//80
            col.Add("Show your ability to ask sales questions that lead to high quality interactions with customers and clients");//81
            col.Add("Demonstrate your ability of managing the relationship between your brand, product/service, price");//82
            col.Add("Demonstrate your understanding of negotiating deals that stick");//83
            col.Add("Demonstrate your understanding of design thinking that creates stronger, clearer, attention grabbing design, and your understanding of the how, why, and when of layout and composition");//84
            col.Add("Show your understanding of typographic practices, what to select, and use to impact and power, what type is measured and sized, and what factors");//85
            col.Add("Demonstrate your ability to avoid pitfalls, improve design processes, and creatively solve problems that emerge when building a brand’s identity");//86
            col.Add("Demonstrate your ability to use and understanding of using industry standard tools to create and edit layouts, images, artwork");//87
            col.Add("Now that we're acquainted I would like to learn a little about your projects.");//88
            col.Add("Your basic info has been updated successfully");//89
            col.Add("Your educational info has been updated successfully");//90
            col.Add("Your profile has been completed, now you can perform the followings");//91
            col.Add("Sorry wrong input ! Please try again");//92
            col.Add("Demonstrate your understanding of the android development environment");//93
            col.Add("Demonstrate your ability of understanding the right method to utilize in order for the user to to acknowledge information, make a confirmation, or be notified of an event");//94
            col.Add("Demonstrate your understanding of using data management and presentation tools of the Android SDK,  shared preferences, JSON-formatted text files, and SQLite to manage data, and customization of data display and handle common events");//95
            col.Add("Illustrate your abilities to use different techniques to create visually compelling animations and screen transitions for Android apps");//96
            col.Add("Demonstrate your understanding of  of unit level testing and built in tools to make it possible");//97
            col.Add("Demonstrate your understanding on packaging and distributing Android apps on the Google Play store");//98
            col.Add("Demonstrate your ability to wirte code, understand Swift’s key concepts, best practices, and program with problem solving in mind");//99
            col.Add("Illustrate your familiartiry with the Xcode development environment used to develop for macOs, iOS, watchOs, and tvOs");//100
            col.Add("Demonstrate your ability and familiarity of developing user interfaces for native iOS applications");//101
            col.Add("Here is a short test to judge how well versed you are about  application architecture of native iOS apps, including the application life cycle, events and tasks, and hierarchies");//102
            col.Add("Demonstrate your understanding of delivering a final application, from the essential steps fo preparing an app for release, to testing and, submitting it to the App Store");//103
            col.Add("Demonstrate your skills in defining variables, functions, and custom classes to working with the C++ standard Template library");//104
            col.Add("Demonstrate your ability to cater to and troubleshoot common challenges faced during  you cater the challenges faced during C++ programming");//105
            col.Add("Show how well are you acquainted with move semantics, a technique for optimizing the transfer of data and reallocating memory without extra copy operations");//106
            col.Add("Demonstrate your ability of smart pointer, objects that ensure memory and other resources are allocated appropriately and efficiently");//107
            col.Add("Should us  how well do you do resource management with custom string libraries in C++");//108
            col.Add("Demonstrate your ability of understanding and applying basic programming concepts such as functions, variables, loops");//109
            col.Add("Show your understanding and ability when it comes to developing scripts");//110
            col.Add("Demonstrate your ability to utilize Tkinter to create graphical user interfaces (GUI) for Python applications");//111
            col.Add("Demonstrate your ability to solve common Python programming challenges");//112
            col.Add("Demonstrate your abilities to utilize the Django Python framework to create data driven web apps");//113
        }

        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {

            if (wait == true)
            {
                wait = false;
                asked();
                ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));
                await HandleSystemMessage(activity, connector);
                var response = Request.CreateResponse(HttpStatusCode.OK);
                wait = true;
                return response;

            }
            else
            {
                var response = Request.CreateResponse(HttpStatusCode.OK);
                return response;
            }

        }

        private async Task<Activity> HandleSystemMessage(Activity activity, ConnectorClient connector)
        {
            try
            {
                
            var replymesge = string.Empty;
            Activity reply = new Activity();
            reply = activity.CreateReply();
            if (activity.Type == ActivityTypes.Message)
            {
                    //if (activity.Text == null)
                    //{                                          
                    //    activity.Text = activity.GetChannelData<dynamic>()?.referral["ref"] ?? activity.Text;
                    //}
                    if (activity.Text.Length <= 499)
                    {
                        
                        activity.Text =  activity.GetChannelData<dynamic>()?.message?.quick_reply?.payload ?? activity.Text;
                        var phrase = activity.Text;
                        var luisresp = await LuisService.ParseUserInput(phrase);
                        //StateClient stateClient = activity.GetStateClient();
                        //BotData userData = await stateClient.BotState.GetUserDataAsync(activity.ChannelId, activity.From.Id);
                        //var sentGreeting = userData.GetProperty<bool>("SentGreeting");
                        //int words = luisresp.query.Split(' ').Length;
                        while (luisresp == null)
                        {
                            luisresp = await LuisService.ParseUserInput(phrase);
                        }

                    back:

                        if (luisresp.intents.Count() > 0)
                        {
                            var str = luisresp.topScoringIntent;
                            try
                            {
                                //if (counter == 10 && flag == 1)
                                //{
                                //    str.intent = "description";
                                //    flag = 0;

                                //}
                                //else if (counter == 11)
                                //{
                                //    str.intent = "title";
                                //}

                                if (counter == 12)
                                {
                                    str.intent = "title";
                                }
                                else if (counter == 13)
                                {
                                    str.intent = "description";
                                }
                                else if (counter == 14)
                                {
                                    str.intent = "technologies";
                                }
                                
                                var symb = string.Empty;
                            next:
                                if (missed == 1)//fb
                                {
                                    str.intent = "misdata";
                                    profile[profile.ToList().IndexOf("Not Specified")] = luisresp.query;
                                    missed = 0;
                                }
                                switch (str.intent)
                                {
                                    case "misdata"://fb
                                        if (missing.Contains("Not Specified"))
                                        {
                                            fbmissing_data();
                                            
                                        }
                                        else
                                        {
                                            replymesge = col.ElementAt(1);
                                            infoConfirm(reply);
                                            des_pro = 0;
                                        }


                                        break;

                                    case "None":

                                        //var validation_result = new Tuple<bool, string>(true, "");//4
                                        //validation_result = input_validation(activity.Text, count);//5
                                        //if (validation_result.Item1)//6
                                        //{

                                            callback = 1;
                                            switch (count)
                                            {
                                                case 1:
                                                    luisresp = await LuisService.ParseUserInput(col.ElementAt(30) + " " + luisresp.query);
                                                    replymesge = luisresp.query;
                                                    break;

                                                case 2:
                                                    luisresp = await LuisService.ParseUserInput(col.ElementAt(31) + " " + luisresp.query);
                                                    replymesge = luisresp.query;
                                                    break;

                                                case 3:
                                                    luisresp = await LuisService.ParseUserInput(col.ElementAt(32) + " " + luisresp.query);
                                                    replymesge = luisresp.query;
                                                    break;

                                                case 4:
                                                    luisresp = await LuisService.ParseUserInput(col.ElementAt(33) + " " + luisresp.query);
                                                    replymesge = luisresp.query;
                                                    break;

                                                case 5:
                                                    luisresp = await LuisService.ParseUserInput(col.ElementAt(34) + " " + luisresp.query);
                                                    replymesge = luisresp.query;
                                                    break;

                                                case 6:
                                                    luisresp = await LuisService.ParseUserInput(col.ElementAt(35) + " " + luisresp.query);
                                                    replymesge = luisresp.query;
                                                    break;

                                                case 7:
                                                    luisresp = await LuisService.ParseUserInput(col.ElementAt(36) + " " + luisresp.query);
                                                    replymesge = luisresp.query;
                                                    break;

                                                case 8:
                                                    luisresp = await LuisService.ParseUserInput(col.ElementAt(37) + " " + luisresp.query);
                                                    replymesge = luisresp.query;
                                                    break;

                                                case 9:
                                                    luisresp = await LuisService.ParseUserInput(col.ElementAt(38) + " " + luisresp.query);
                                                    replymesge = luisresp.query;
                                                    break;
                                                
                                                case 10:
                                                    luisresp = await LuisService.ParseUserInput(col.ElementAt(39) + " " + luisresp.query);
                                                    replymesge = luisresp.query;
                                                    break;

                                                case 11:
                                                    luisresp = await LuisService.ParseUserInput(col.ElementAt(40) + " " + luisresp.query);
                                                    replymesge = luisresp.query;
                                                    break;

                                                case 12:
                                                    luisresp = await LuisService.ParseUserInput(col.ElementAt(38) + " " + luisresp.query);
                                                    replymesge = luisresp.query;
                                                    break;
                                             }
                                        //}
                                        //else//7
                                        //{
                                        //    switch (count)
                                        //    {
                                        //        case 1:
                                        //        case 2:
                                        //        case 4:
                                        //        case 5:
                                        //        case 7:
                                        //        case 9:
                                        //            replymesge = col.ElementAt(92) + " using alphabets only..." + "\n" + validation_result.Item2;
                                        //            break;
                                        //        case 3:
                                        //        case 8:
                                        //        case 10:
                                        //            replymesge = col.ElementAt(92) + " using numbers only..." + "\n" + validation_result.Item2;
                                        //            break;
                                        //        case 6:
                                        //            replymesge = col.ElementAt(92) + " e.g. example@smbi.ca " + "\n" + validation_result.Item2;
                                        //            break;

                                        //        default:
                                        //            replymesge = col.ElementAt(92) + "\n" + validation_result.Item2;
                                        //            break;
                                        //    }
                                        //}
                                        break;

                                    case "questions":
                                        callback = 0;
                                        //replymesge = luisresp.query;
                                        if (luisresp.entities.Length == 0)
                                        {
                                            entity = luisresp.query;
                                        }
                                        else
                                        {
                                            entity = luisresp.entities[0].entity.ToLower();
                                        }


                                        if (!avoid.Contains(entity))
                                        {
                                            repeat = 1;
                                            if (entity == "started")
                                            {
                                                avoid.Clear();
                                            }
                                            else
                                            {
                                                avoid.Add(entity);
                                            }
                                            if (entity.Substring(0, 4) == "mode")
                                            {
                                                entity = entity.Substring(0, 4);
                                            }
                                            else if (entity.Substring(0, 4) == "modu")
                                            {
                                                entity = entity.Substring(0, 6);
                                            }
                                            else if (entity.Substring(0, 4) == "deve")
                                            {
                                                entity = entity.Substring(0, 9);
                                            }
                                            else if (entity.Substring(0, 4) == "mark")
                                            {
                                                entity = entity.Substring(0, 8);
                                            }
                                            else if (entity.Substring(0, 4) == "info")
                                            {
                                                entity = entity.Substring(0, 11);
                                            }

                                            else if (avoid.Contains("data") || avoid.Contains("update"))
                                            {
                                                avoid.Remove("data");
                                                avoid.Remove("update");
                                            }
                                            else if (avoid.Contains("thanks"))
                                            {
                                                avoid.Add("another");
                                            }
                                            else if (avoid.Contains("test"))
                                            {
                                                avoid.Remove("test");
                                            }
                                            else if (avoid.Contains("another"))
                                            {
                                                int i = 0;
                                                while (avoid.Contains("mode" + i))
                                                {
                                                    avoid.Remove("mode" + i);
                                                    i++;
                                                }
                                            }

                                            switch (entity)
                                            {
                                                case "started":
                                                    Initialize_variables();
                                                    data.Add(reply.Recipient.Id);
                                                    reply.Text = "Hi. I am Maaz created by the folks at TalentHome to help you find the right design,development, sales, or marketing position with today's fastest growing companies. Shall we get going ?";
                                                    await connector.Conversations.ReplyToActivityAsync(reply);
                                                    Thread.Sleep(500);
                                                    reply.Text = "";
                                                    replymesge = col.ElementAt(0);
                                                    continueornot(reply);
                                                   // RedirectController.cont = reply;
                                                                               
                                                                                                    
                                                    



                                                    //reply.Text = col.ElementAt(0); 
                                                    //await connector.Conversations.ReplyToActivityAsync(reply);
                                                    //Thread.Sleep(500);
                                                    //replymesge = col.ElementAt(1); 
                                                    break;

                                                case "matched":
                                                    replymesge = col.ElementAt(1);
                                                    break;

                                                case "profile":// case "suited":
                                                    string[] occup = luisresp.query.Split();
                                                    data.Add(occup[0]);
                                                    //occupation = occup[0];
                                                    replymesge = col.ElementAt(3);
                                                    reffered_to_choice(occup[0], reply);
                                                    break;

                                                case "interested": //// interested
                                                    data.Add(luisresp.query.Substring(25));
                                                    replymesge = col.ElementAt(4);
                                                    job_position(reply);
                                                    break;


                                                case "position":
                                                    data.Add(luisresp.query.Substring(28));
                                                    replymesge = col.ElementAt(5);
                                                    break;

                                                case "company":
                                                    reply.Text = col.ElementAt(10);
                                                    await connector.Conversations.ReplyToActivityAsync(reply);
                                                    replymesge = col.ElementAt(11);
                                                    formal_education(reply);
                                                    break;

                                                case "formal":
                                                    replymesge = col.ElementAt(12);
                                                    break;

                                                case "basic":
                                                    des_pro = 1;
                                                    count = 7;
                                                    counter = 7;
                                                    data.Clear();
                                                    for (int i = 0; i <= 6; i++)
                                                    {
                                                        data.Add(profile[i]);
                                                    }
                                                    check_status(1);

                                                    reply.Text = col.ElementAt(8);
                                                    await connector.Conversations.ReplyToActivityAsync(reply);
                                                    // Thread.Sleep(2000);
                                                    replymesge = col.ElementAt(9);

                                                    break;
                                                case "details":
                                                    des_pro = 1;
                                                    replymesge = col.ElementAt(2);
                                                    count = 1;
                                                    break;
                                                //case "company":
                                                //    replymesge = col.ElementAt(13);
                                                //    selectCompany(reply);
                                                //    break;
                                                                                                   
                                                    //// case "position":
                                                    //// replymesge = col.ElementAt(5); 
                                               ////     break;

                                                case "designer":
                                                    //// string ski = luisresp.query.Substring(23);
                                                    //// data.Add(luisresp.query.Substring(23));
                                                    //// ski = null;
                                                    //// replymesge = col.ElementAt(4);

                                                    string ski = luisresp.query.Substring(23);
                                                    data.Add(ski);
                                                    ski = null;
                                                    replymesge = col.ElementAt(15);
                                                    break;

                                                case "developer":
                                                    ski = luisresp.query.Substring(24);
                                                    data.Add(ski);
                                                    ski = null;
                                                    replymesge = col.ElementAt(15);
                                                    break;

                                                case "marketer":
                                                    ski = luisresp.query.Substring(24);
                                                    data.Add(ski);
                                                    ski = null;
                                                    replymesge = col.ElementAt(15);
                                                    break;

                                                case "module":
                                                    replymesge = module_types(reply, luisresp, replymesge);
                                                    DerivedActivity derive = new DerivedActivity(reply);
                                                    reply.Attachments.Clear();
                                                    reply.AttachmentLayout = null;
                                                    if (reply.Text != null)
                                                    {
                                                        await connector.Conversations.ReplyToActivityAsync(reply);//new
                                                        Thread.Sleep(500);
                                                    }
                                                    reply.Attachments = derive.Attachments;
                                                    reply.AttachmentLayout = derive.AttachmentLayout;
                                                    break;

                                                case "before":
                                                    count = 5;
                                                    counter = 5;
                                                    replymesge = col.ElementAt(13);
                                                    
                                                    break;

                                                case "schooling":
                                                    reply.Text = col.ElementAt(18);
                                                    await connector.Conversations.ReplyToActivityAsync(reply);//new
                                                    replymesge = col.ElementAt(20);
                                                    proj_prof(reply);
                                                    break;

                                                case "professional":
                                                    count = 9;
                                                    counter = 9;
                                                    reply.Text = col.ElementAt(21);
                                                    await connector.Conversations.ReplyToActivityAsync(reply);//new
                                                    replymesge = col.ElementAt(22);
                                                    break;

                                                case "project":
                                                    replymesge = col.ElementAt(26);
                                                    count = 12;
                                                    counter = 12;
                                                     break;

                                                case "test":
                                                                                                      
                                                    reply.Text = Regex.Replace(activity.Text.Substring(22), "[0-9]+", string.Empty); 
                                                    //reply.Text = Regex.Replace(reply.Text, "(\\B[A-Z])", " $1");
                                                    replymesge = giving_test(reply, replymesge);
                                                    if (Regex.Replace(activity.Text.Substring(22), "[^0-9]+", string.Empty) != "")
                                                    {
                                                        reply.Text = col.ElementAt(int.Parse(Regex.Replace(activity.Text.Substring(22), "[^0-9]+", string.Empty)));

                                                        derive = new DerivedActivity(reply);
                                                        reply.Attachments.Clear();
                                                        reply.AttachmentLayout = null;
                                                        if (reply.Text != null)
                                                        {
                                                            await connector.Conversations.ReplyToActivityAsync(reply);//new
                                                            Thread.Sleep(500);
                                                        }
                                                        reply.Attachments = derive.Attachments;
                                                        reply.AttachmentLayout = derive.AttachmentLayout;
                                                    }
                                                    
                                                    break;

                                                case "scores":
                                                    var display_scores = JsonConvert.DeserializeObject<MongoData>(MongoUser.exist_user(id).ToJson());
                                                    foreach (var sc in display_scores.test)
                                                    {
                                                        //   replymesge = "Technology :" + " " + sc.technology + "\n\n" +
                                                        //               "Score :" + " " + sc.score;
                                                    }
                                                    break;


                                                case "another":
                                                    replymesge = col.ElementAt(41);
                                                    choose_tech(reply);
                                                    break;

                                                case "thanks":
                                                    reply.Text = col.ElementAt(45);
                                                    await connector.Conversations.ReplyToActivityAsync(reply);//new
                                                    replymesge = col.ElementAt(91);
                                                    commands(reply);
                                                    break;


                                                case "data":
                                                    reply.Text = display_user_info(1);
                                                    await connector.Conversations.ReplyToActivityAsync(reply);//new
                                                    // Thread.Sleep(2000);
                                                    replymesge = col.ElementAt(91);
                                                    commands(reply);
                                                    break;


                                                case "update":
                                                    replymesge = col.ElementAt(47);
                                                    update_profile(reply);

                                                    break;

                                                case "information":
                                                    update_info = 2;
                                                    if (luisresp.entities[0].entity.Substring(11).ToLower() == "basic")
                                                    {
                                                        des_pro = 1;
                                                        replymesge = col.ElementAt(2);
                                                        count = 1;
                                                        counter = 1;
                                                        str.intent = "firstName";

                                                    }
                                                    else if (luisresp.entities[0].entity.Substring(11).ToLower() == "educa")
                                                    {
                                                        des_pro = 1;
                                                        replymesge = col.ElementAt(9);
                                                        count = 7;
                                                        counter = 7;
                                                        str.intent = "university";

                                                    }

                                                    break;

                                                case "mode":

                                                    if (ques_count <= display.Count())
                                                    {
                                                        var ans = activity.Text.Substring(0, 1);
                                                        if (display[ques_count].Answers == ans)
                                                        {
                                                            score++;
                                                        }
                                                        ++ques_count;
                                                        if (ques_count < display.Count())
                                                        {
                                                            replymesge = display[ques_count].Statements;
                                                            test(reply, display[ques_count].Options, ques_count);
                                                            // Thread.Sleep(1100);
                                                        }
                                                        else
                                                        {
                                                            data.Add(score.ToString());
                                                            reply.Text = "You have completed this test.Your results are as follows";
                                                            await connector.Conversations.ReplyToActivityAsync(reply);
                                                            reply.Text = "Track:" + " " + data[2] + "\n\n" +
                                                                         "Module Name:" + " " + data[3] + "\n\n";
                                                            if (data.Count == 6)
                                                            {
                                                                reply.Text = reply.Text + "Technology:" + " " + data[4] + "\n\n"
                                                                                        + "Score:" + " " + data[5];
                                                            }
                                                            else
                                                            {
                                                                reply.Text = reply.Text + "Score:" + " " + data[4];

                                                            }

                                                            await connector.Conversations.ReplyToActivityAsync(reply);
                                                            //   Thread.Sleep(1000);
                                                            data.RemoveAt(2);
                                                            reply.Text = "You have completed this test.Your results are as follows";
                                                            save_user_test(flagi);
                                                            replymesge = col.ElementAt(44);
                                                            test_again(reply);

                                                        }
                                                    }

                                                    break;
                                            }
                                        }
                                        else
                                        {
                                            repeat = 0; 
                                            avoid.Remove(avoid.Last());
                                        }
                                        break;

                                    default:
                                        callback = 0;
                                        switch (counter)
                                        {
                                            case 1:
                                                check_entity(symb, luisresp);
                                                replymesge = correctSequence(str.intent, replymesge, 2);
                                                job_options(reply);
                                                break;

                                            case 2:
                                                check_entity(symb, luisresp);
                                                replymesge = correctSequence(str.intent, replymesge, 6);
                                                break;

                                            case 3:
                                                check_entity(symb, luisresp);
                                                replymesge = correctSequence(str.intent, replymesge, 7);
                                                break;
                                            case 4:
                                                check_entity(symb, luisresp);
                                                // replymesge = correctSequence(str.intent, replymesge, 8);
                                                RedirectController.cont = reply;            
                                                personality_view(reply);
                                                break;

                                            case 5:
                                                check_entity(symb, luisresp);
                                                replymesge = correctSequence(str.intent, replymesge, 14);
                                                break;

                                            case 6:
                                                check_entity(symb, luisresp);
                                                replymesge = correctSequence(str.intent, replymesge, 15);
                                                break;
                                            //if (already == 1)
                                            //{
                                            //    already = 0;
                                            //    reply.Text = "Welcome" + " " + profile[1] + "! Your Basic profile has been saved.";
                                            //    await connector.Conversations.ReplyToActivityAsync(reply);
                                            //    // Thread.Sleep(1000);
                                            //    reply.Text = display_user_info(counter);
                                            //    await connector.Conversations.ReplyToActivityAsync(reply);

                                            //}


                                            //if (counter == 6)
                                            //{
                                            //    if (data.Count == 6)
                                            //    {
                                            //        check_entity(symb, luisresp);
                                            //        if (data.Count == 7)
                                            //        {
                                            //            if (update_info == 2)
                                            //            {
                                            //                check_status(2);
                                            //                update_info = 0;
                                            //                reply.Text = col.ElementAt(89);
                                            //                await connector.Conversations.ReplyToActivityAsync(reply);
                                            //                // Thread.Sleep(2000);
                                            //                replymesge = col.ElementAt(91);
                                            //                commands(reply);

                                            //            }
                                            //            else
                                            //            {
                                            //                check_status(0);
                                            //                reply.Text = correctSequence(str.intent, replymesge, 8);
                                            //                await connector.Conversations.ReplyToActivityAsync(reply);
                                            //                //  Thread.Sleep(2000);
                                            //                replymesge = col.ElementAt(14);
                                            //            }
                                            //        }
                                            //    }
                                            //    else
                                            //    {
                                            //reply.Text = correctSequence(str.intent, replymesge, 8);
                                            //await connector.Conversations.ReplyToActivityAsync(reply);
                                            ////Thread.Sleep(2000);
                                            //replymesge = col.ElementAt(14);
                                            //break;
                                            //    }
                                            //}


                                            // break;

                                            case 7:
                                                check_entity(symb, luisresp);
                                                replymesge = correctSequence(str.intent, replymesge, 16);
                                                break;

                                            case 8:
                                                check_entity(symb, luisresp);
                                                replymesge = correctSequence(str.intent, replymesge, 17);
                                                yesorno(reply);
                                                break;

                                            case 9:
                                                check_entity(symb, luisresp);
                                                replymesge = correctSequence(str.intent, replymesge, 23);
                                                //if (update_info != 2)
                                                //{
                                                //    replymesge = correctSequence(str.intent, replymesge, 12);
                                                //    flag = 0;
                                                //    yesorno(reply);
                                                //}
                                                //else
                                                //{
                                                //    var res = JsonConvert.DeserializeObject<MongoData>(MongoUser.exist_user(id).ToJson());

                                                //    if (res.educational.Count() > des_pro)
                                                //    {
                                                //        replymesge = col.ElementAt(9);
                                                //        count = 7;
                                                //        des_pro++;
                                                //    }
                                                //    else
                                                //    {
                                                //        des_pro = 1;
                                                //        edu_pro_record(2);
                                                //        reply.Text = col.ElementAt(90);
                                                //        await connector.Conversations.ReplyToActivityAsync(reply);
                                                //        // Thread.Sleep(2000);
                                                //        replymesge = col.ElementAt(91);
                                                //        commands(reply);
                                                //        update_info = 0;
                                                //    }
                                                //}
                                                break;

                                            case 10:
                                                check_entity(symb, luisresp);
                                                replymesge = correctSequence(str.intent, replymesge, 24);
                                                //if (counter == 10 && data.Count >= 6)
                                                //{

                                                //    check_entity(symb, luisresp);
                                                //    edu_pro_record(0);
                                                //}

                                                //if (already == 1)
                                                //{
                                                //    already = 0;
                                                //    reply.Text = "Welcome" + " " + profile[1] + "! Your Basic,Educational and Professional profile has been saved.";
                                                //    await connector.Conversations.ReplyToActivityAsync(reply);
                                                //    //   Thread.Sleep(2000);
                                                //    reply.Text = display_user_info(counter);
                                                //    await connector.Conversations.ReplyToActivityAsync(reply);
                                                //    //  Thread.Sleep(2000);
                                                //    reply.Text = col.ElementAt(88);
                                                //    await connector.Conversations.ReplyToActivityAsync(reply);

                                                //}

                                                //var number = JsonConvert.DeserializeObject<MongoData>(MongoUser.exist_user(id).ToJson());

                                                //if (number.project == null)
                                                //{
                                                //    replymesge = "sorry wrong input";
                                                //}
                                                //else
                                                //{

                                                //    if (pro_count > 0 && pro_count <= Convert.ToInt16(number.project.no_of_projects))
                                                //    {
                                                //        if (pro_count > 1)
                                                //        {
                                                //            data.Add(luisresp.query);
                                                //        }

                                                //        replymesge = correctSequence("projects", replymesge, 16) + " " + pro_count.ToOrdinalWords() + " project ?";
                                                //    }
                                                //    else
                                                //    {
                                                //        data.Add(luisresp.query);
                                                //        pro_details();
                                                //        reply.Text = col.ElementAt(39);
                                                //        await connector.Conversations.ReplyToActivityAsync(reply);
                                                //        //   Thread.Sleep(1000);
                                                //        reply.Text = col.ElementAt(40);
                                                //        await connector.Conversations.ReplyToActivityAsync(reply);
                                                //        //   Thread.Sleep(1000);
                                                //        replymesge = taking_tests(number, reply, replymesge);
                                                //        count = count + 2;
                                                //        counter = counter + 2;
                                                //    }
                                                //}
                                                break;

                                            case 11:
                                                check_entity(symb, luisresp);
                                                replymesge = correctSequence(str.intent, replymesge, 25);
                                                //var numb = JsonConvert.DeserializeObject<MongoData>(MongoUser.exist_user(id).ToJson());

                                                //if (numb.project == null)
                                                //{
                                                //    replymesge = "sorry wrong input";
                                                //}
                                                //else
                                                //{
                                                //    if (pro_count > 0 && pro_count <= Convert.ToInt16(numb.project.no_of_projects))
                                                //    {

                                                //        if (pro_count == 1)
                                                //        {
                                                //            data.Add(luisresp.query);
                                                //        }
                                                //        else
                                                //        {
                                                //            data.Add(luisresp.query);
                                                //        }

                                                //        replymesge = correctSequence("title", replymesge, 17) + " " + pro_count.ToOrdinalWords() + " project";
                                                //        flag = 1;
                                                //        pro_count++;
                                                //        count = count - 2;
                                                //        counter = counter - 2;
                                                //    }
                                                //    else
                                                //    {

                                                //    }
                                                //}
                                                break;

                                            case 12:
                                                check_entity(symb, luisresp);
                                                replymesge = correctSequence(str.intent, replymesge, 27);
                                                //if (des_pro == 2)
                                                //{
                                                //    reply.Text = col.ElementAt(39);
                                                //    await connector.Conversations.ReplyToActivityAsync(reply);
                                                //    //   Thread.Sleep(1000);
                                                //    reply.Text = col.ElementAt(40);
                                                //    await connector.Conversations.ReplyToActivityAsync(reply);
                                                //    //   Thread.Sleep(1000);
                                                //    var num = JsonConvert.DeserializeObject<MongoData>(MongoUser.exist_user(id).ToJson());
                                                //    replymesge = taking_tests(num, reply, replymesge);

                                                //}
                                                //else
                                                //{
                                                //    replymesge = "sorry wrong input";
                                                //}

                                                break;

                                            case 13:
                                                check_entity(symb, luisresp);
                                                replymesge = correctSequence(str.intent, replymesge, 28);
                                                //replymesge = col.ElementAt(91);
                                                //commands(reply);
                                                break;

                                            case 14:
                                                check_entity(symb, luisresp);
                                                replymesge = correctSequence(str.intent, replymesge, 29);
                                                //replymesge = col.ElementAt(91);
                                                //commands(reply);
                                                break;
                                        }
                                        break;

                                }
                                if (callback == 1)
                                {
                                    callback = 0;
                                    goto back;
                                }
                                if (symb != string.Empty)
                                {
                                    data.Add(symb);
                                }

                            }
                            catch (FacebookApiException ex)
                            {
                                replymesge = ex.Message.ToString();
                            }
                            if (repeat == 1)
                            {
                               reply.Text = replymesge;
                               await connector.Conversations.ReplyToActivityAsync(reply);
                            }

                        }

                        else
                        {
                            replymesge = $"sorry.I could not get as to what you say";
                        }
                    }
                    else
                    {
                        reply.Text = "Sorry wrong input! Please try again";
                    }
            }

            else if (activity.Type == ActivityTypes.DeleteUserData)
            {
                avoid.Clear();
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (activity.Type == ActivityTypes.ConversationUpdate)
            {
                count = 1;
                counter = 1;
                data.Clear();
                avoid.Clear();
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
            }
            else if (activity.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (activity.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (activity.Type == ActivityTypes.Ping)
            {
            }
        }
        catch(Exception ex)
        {
            ex.Message.ToString();
            //  ex.Data["err"] = "Something went wrong.Please delete your all conversation and Get Started again";
            //  PromptDialog.Text(context, AfterUserInputSymbol, ex.Data["err"].ToString(), "Try again message", 3);
            //  await context.PostAsync(ex.Data["err"].ToString());                        
        }
            return null;
    }

        private void Initialize_variables()
        {
                userid = null;
                acctoken = null;
                data.Clear();
                avoid.Clear();
                profile = new string[7];
                missing = new string[7];
                missed = 0;
                already = 0;
                count = 1;
                flag = 0;
                counter = 1;
                callback = 0;
                des_pro = 0;
                update_info = 0;
                id = null;
                occupation = null;
                entity = null;
                pro_count = 1;
                //wait = true;
                ques_count = 0;
                score = 0;
                flagi = 0;
                repeat = 1;
                testsretrieved = null;
                display = null;
                  
            
        }

    private async Task<Tuple<LuisResponse, string>> CallLuisAgain(LuisResponse luisresp, string reply, int y)
        {
            luisresp = await LuisService.ParseUserInput(col.ElementAt(y) + " " + luisresp.query);
            reply = luisresp.query;
            var tuple = new Tuple<LuisResponse, string>(luisresp, reply);
            return tuple;
        }

        private void check_entity(string symb, LuisResponse luisresp)
        {
            try
            {
                if (luisresp.entities.Length == 0)
                {
                    symb = luisresp.query;
                }
                else
                {
                    symb = luisresp.entities[0].entity.ToLower();
                }
           
                if (symb != string.Empty)
                {
                    data.Add(symb);
                }
            }
            catch  // handle entity if not found
            {


            }
        }

        private string fbmissing_data()//fb
        {
            int k;
            string text = null;
            for (k = 0; k <= 6; k++)
            {
                if (missing[k] != null)
                {
                    missed = 1;
                    missing[k] = null;
                    break;
                }
            }
            switch (k)
            {
                case 1:
                    text = col.ElementAt(2);
                    break;

                case 2:
                    text = col.ElementAt(3);
                    break;
                case 3:
                    text = col.ElementAt(4);
                    break;

                case 4:
                    text = col.ElementAt(5);
                    break;
                case 5:
                    text = col.ElementAt(6);
                    break;
                case 6:
                    text = col.ElementAt(7);
                    break;
            }

            return text;

        }

        private void save_user_test(int flagi)
        {
            int j = 1;
            if (data.Count < 5)
            {
              var val =  data.ElementAt(3);
              data.Insert(3,"Null");
                data.Add(val);
            }
            var record = new BsonDocument
            {
                { "mod_no" ,   data.ElementAt(j++) },
                { "mod_name" , data.ElementAt(j++) },
                { "technology" , data.ElementAt(j++) },
                { "score"      , data.ElementAt(j++) },
            };

            MongoUser.upd_test_info(record,flagi,4,id);
            data.Clear();
            data.Add(id);
        }

        private void pro_details()
        {

            int cou = data.Count;//3 // 5// 7
            cou = (cou - 1) / 2; // 3 -1 = 2 // 5-1 = 4 // 7-1 = 6

            int j = 1;
            List<BsonDocument> multiple = new List<BsonDocument>();
            for (int i = 0; i < cou; i++)
            {
                var record = new BsonDocument
                {
                     { "title"    , data.ElementAt(j++) },
                     { "description"   , data.ElementAt(j++) },
                };
                multiple.Add(record);
            }

            MongoUser.upd_project_info(multiple, 3, id);
            data.Clear();
            data.Add(id);
        }

        private void edu_pro_record(int upd)
        {

            int cou = data.Count;//10 // 7// 6
            if (upd == 2)
            {
                cou = cou + 3;
            }
            cou = cou - 3; // 10 -3 = 7 // 7-3 = 4 // 6-3 = 3
            cou = (cou - 1) / 3;

            int j = 1;
            List<BsonDocument> multiple = new List<BsonDocument>();
            for (int i = 0; i < cou; i++)
            {
                var record = new BsonDocument
                        {
                           { "uni_name"    , data.ElementAt(j++) },
                           { "pass_year"   , data.ElementAt(j++) },
                           { "degree_name" , data.ElementAt(j++) }
                        };

                multiple.Add(record);
            }
            if (upd == 2)
            {
                MongoUser.add_edu_infos(multiple, id);
            }
            else
            {
                var com = data.ElementAt(j++);
                var pos = data.ElementAt(j++);
                var proj = new BsonDocument
                {
                {"no_of_projects",data.ElementAt(j++)}
                };
                MongoUser.add_edu_infos(multiple, id);
                MongoUser.upd_pro_info(com, pos, proj, 2, id);

            }
          
            data.Clear();
            data.Add(id);
        }

        private bool check_status(int upd)
        {
            int stat = 1;
            if (id != null || occupation != null)
            {            
                if (upd != 2)
                {
                    var record = new BsonDocument
                    {
                       {"_id" , data.ElementAt(0)}
                    };
                    var chek = MongoUser.add_basic_info(record);
                }
                else
                {
                    var res = JsonConvert.DeserializeObject<MongoData>(MongoUser.exist_user(id).ToJson());
                    stat = res.basic.status;
                    occupation = res.professional.occupation_type;
                }             
                var info = new BsonDocument
                {    
                        { "first_name" , data.ElementAt(1) },
                        { "last_name"  , data.ElementAt(2) },
                        { "age"        , "25" },//Convert.ToInt32(data.ElementAt(3)),
                        { "gender"     , data.ElementAt(4) },
                        { "location"   , data.ElementAt(5) },
                        { "email"      , data.ElementAt(6) },
                        { "status"     , stat              }                                  
                };
                var pro = new BsonDocument
                {       
                  { "occupation_type" , occupation}                                    
                };

                MongoUser.upd_basic_info(info,pro,upd,id); 
                occupation = null;
                flag = 0;
                data.Clear();
                data.Add(id);

                               
            }
            return false;
        }

        private string correctSequence(string intent, string reply, int x)
        {
            //if (des_pro == 1)
            //{

                if (Enum.GetName(typeof(decision), counter) == intent)
                {
                    reply = col.ElementAt(x);
                    count++;
                    counter++;
                }
                else
                {
                    reply = "Sorry wrong input" + "\n\n" + col.ElementAt(x - 1);
                }
            //}
            //else
            //{
            //    reply = "Sorry wrong input";
            //    data.Remove(data.Last());

            //}
            return reply;
        }

        private string display_user_info(int cas)
        {
            string display = null;

            var res = JsonConvert.DeserializeObject<MongoData>(MongoUser.exist_user(id).ToJson());

            if (cas == 6 || cas == 10 || cas == 1)
            {

                   display = "Basic Profile:" + "\n\n" + "\n\n" +
                             "First Name:" + res.basic.first_name + "\n\n" + 
                             "Last Name:" + res.basic.last_name + "\n\n" + 
                             "Gender:" + res.basic.gender + "\n\n" + 
                             "Location:" + res.basic.location + "\n\n" + 
                             "Email:" + res.basic.email;
              
                    if (cas == 10 || cas == 1)
                    {
                    display = display + "\n\n" + "\n\n" + "Educational Profile:";

                    foreach (var ed in res.educational)
                    {
                            display = display + "\n\n" + "\n\n" +
                                     "University :" + " " + ed.uni_name + "\n\n" +
                                     "Passing Year :" + " " + ed.pass_year + "\n\n" +
                                     "Degree :" + " " + ed.degree_name;
                    }
                    display = display + "\n\n" + "\n\n" +
                             "Professional Profile:" + "\n\n" +
                             "Occupation:" + res.professional.occupation_type + "\n\n" + 
                             "Company:" + res.professional.company_type + "\n\n" +
                             "Position:" + res.professional.position_type;
                    if (cas == 1)
                    {
                        display = display + "\n\n" + "\n\n" +
                                  "Projects:" + "\n\n" +
                                  "Total Projects :" + res.project.no_of_projects;
                        foreach (var dt in res.project.details)
                        {
                            display = display + "\n\n" + "\n\n" +
                                      "Title :" + dt.title + "\n\n" +
                                      "Description :" + dt.description;
                        }
                    }
                }                                     
            }
            return display;
        }

        protected void formal_education(Activity reply)
        {
            reply.Attachments = new List<Attachment>();
            List<CardAction> cardButtons = new List<CardAction>();
            CardAction Button1 = new CardAction()
            {
                Value = "let us know about your formal education",
                Type = "postBack",
                Title = "Yes"
            };
            CardAction Button2 = new CardAction()
            {
                Value = "Let's get to fixing your details. What's your first name ?",
                Type = "postBack",
                Title = "No",

            };

            cardButtons.Add(Button1);
            cardButtons.Add(Button2);

            HeroCard jobCard = new HeroCard()
            {
                Buttons = cardButtons
            };

            Attachment jobAttachment = jobCard.ToAttachment();
            reply.Attachments.Add(jobAttachment);
            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
        }
        protected void infoConfirm(Activity reply)
        {
            reply.Attachments = new List<Attachment>();
            List<CardAction> cardButtons = new List<CardAction>();
            CardAction Button1 = new CardAction()
            {
                Value = "Than save the user's basic information ?",
                Type = "postBack",
                Title = "Correct"
            };
            CardAction Button2 = new CardAction()
            {
                Value = "Let's get to fixing your details. What's your first name ?",
                Type = "postBack",
                Title = "Incorrect",

            };

            cardButtons.Add(Button1);
            cardButtons.Add(Button2);

            HeroCard jobCard = new HeroCard()
            {
                Buttons = cardButtons
            };
         
            Attachment jobAttachment = jobCard.ToAttachment();
            reply.Attachments.Add(jobAttachment);
            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
        }
        protected void education_level(Activity reply)
        {
            reply.Attachments = new List<Attachment>();
            List<CardAction> cardButtons = new List<CardAction>();
            CardAction Button1 = new CardAction()
            {
                Value = "",
                Type = "postBack",
                Title = "PostGraduate"
            };
            CardAction Button2 = new CardAction()
            {
                Value = "",
                Type = "postBack",
                Title = "Graduate",

            };

            cardButtons.Add(Button1);
            cardButtons.Add(Button2);

            HeroCard jobCard = new HeroCard()
            {
                Buttons = cardButtons
            };

            Attachment jobAttachment = jobCard.ToAttachment();
            reply.Attachments.Add(jobAttachment);
            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
        }

        protected void personality_test(Activity reply)
        {
            reply.Attachments = new List<Attachment>();
            List<CardAction> cardButtons = new List<CardAction>();
            List<CardImage>  cardimages = new List<CardImage>();

            CardAction Button1 = new CardAction()
            {
                Value = "we would like to know about your personality",
                Type = "postBack",
                Title = "Personality Test"
            };
            CardImage image1 = new CardImage()
            {

            };
            cardButtons.Add(Button1);
            cardimages.Add(image1);
            HeroCard jobCard = new HeroCard()
            {
                Buttons = cardButtons,
                Images = cardimages
            };

            Attachment jobAttachment = jobCard.ToAttachment();
            reply.Attachments.Add(jobAttachment);
            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
        }





        private string giving_test(Activity reply , string replymesge) 
        {
            des_pro = 0;
            var usertest = JsonConvert.DeserializeObject<MongoData>(MongoUser.exist_user(id).ToJson());
            if (usertest.test != null)
            {
                var check = usertest.test.Where(x => x.technology.ToLower() == reply.Text).ToArray();
                if (check.Count() != 0)
                {
                    replymesge = col.ElementAt(46);
                    flagi = 0;
                }
                else
                {
                    data.Add(reply.Text);
                    flagi = 2;
                }
            }
            else
            {
                data.Add(reply.Text);
                flagi = 1;
            }
            if (flagi != 0)
            {               
                testsretrieved = JsonConvert.DeserializeObject<Testing>(MongoUser.ret_sel_test(reply.Text).ToJson());
                display = (testsretrieved.record.Where(x => x.type == reply.Text)).ToArray();
                replymesge = display[0].Statements;
                reply.Text = null;          
                test(reply, display[0].Options, 0);
            }
            return replymesge;
        }

        private void test_again(Activity reply)
        {
            reply.Attachments = new List<Attachment>();
            List<CardAction> cardButtons = new List<CardAction>();
            CardAction Button1 = new CardAction()
            {
                Value = "You want to give another language test",
                Type = "postBack",
                Title = "Yes"
            };
            CardAction Button2 = new CardAction()
            {
                Value = "OK Thanks for visiting our bot",
                Type = "postBack",
                Title = "No",
            };

            cardButtons.Add(Button1);
            cardButtons.Add(Button2);
            HeroCard jobCard = new HeroCard()
            {
                Buttons = cardButtons,
            };

            Attachment jobAttachment = jobCard.ToAttachment();
            reply.Attachments.Add(jobAttachment);
            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
        }

        private void yesorno(Activity reply)
        {
            reply.Attachments = new List<Attachment>();
            List<CardAction> cardButtons = new List<CardAction>();
            CardAction Button1 = new CardAction()
            {
                Value = "What did did you study before ?",
                Type = "postBack",
                Title = "Yes"
            };
            CardAction Button2 = new CardAction()
            {
                Value = "Now that I know about your schooling",
                Type = "postBack",
                Title = "No",
            };

            cardButtons.Add(Button1);
            cardButtons.Add(Button2);
            HeroCard jobCard = new HeroCard()
            {
                Buttons = cardButtons,
            };

            Attachment jobAttachment = jobCard.ToAttachment();
            reply.Attachments.Add(jobAttachment);
            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
        }

        private  string taking_tests(MongoData number,Activity reply ,string replymesge)
        {       
            if (number.professional.occupation_type == "Design")
            {
                replymesge = "Before we can start matching you with jobs related to " + number.professional.position_type + " designer " + col.ElementAt(49);
                design_types(reply, number.professional.position_type);
            }
            else if (number.professional.occupation_type == "Development")
            {
                replymesge = "Before we can start matching you with jobs related to " + number.professional.position_type + " developer " + col.ElementAt(49);
                development_types(reply, number.professional.position_type);
            }
            else
            {
                replymesge = "Before we can start matching you with jobs related to " + number.professional.position_type + " developer " + col.ElementAt(49);
                development_types(reply, number.professional.position_type);
            }
            return replymesge;
        }

        private void commands(Activity reply)
        {
            reply.Attachments = new List<Attachment>();
            List<CardAction> cardButtons = new List<CardAction>();
            CardAction Button1 = new CardAction()
            {
                Value = "your stored data has been displayed",
                Type = "postBack",
                Title = "View Profile"
            };
            CardAction Button2 = new CardAction()
            {
                Value = "ok you want to update your profile",
                Type = "postBack",
                Title = "Update Profile",
            };
            CardAction Button3 = new CardAction()
            {
                Value = "What university did you attend ?",
                Type = "postBack",
                Title = "Help",
            };

            cardButtons.Add(Button1);
            cardButtons.Add(Button2);
            cardButtons.Add(Button3);
            HeroCard jobCard = new HeroCard()
            {
                Buttons = cardButtons,
            };

            Attachment jobAttachment = jobCard.ToAttachment();
            reply.Attachments.Add(jobAttachment);
            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
        }

        private void update_profile(Activity reply)
        {
            reply.Attachments = new List<Attachment>();
            List<CardAction> cardButtons = new List<CardAction>();
            CardAction Button1 = new CardAction()
            {
                Value = "we will edit your informationbasic",
                Type = "postBack",
                Title = "Basic Info"
            };
            CardAction Button2 = new CardAction()
            {
                Value = "we will edit your informationeduca",
                Type = "postBack",
                Title = "Educational Info",
            };
         
            cardButtons.Add(Button1);
            cardButtons.Add(Button2);
            HeroCard jobCard = new HeroCard()
            {
                Buttons = cardButtons,
            };

            Attachment jobAttachment = jobCard.ToAttachment();
            reply.Attachments.Add(jobAttachment);
            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
        }

        private void select_design(Activity reply)
        {
            reply.Attachments = new List<Attachment>();
            List<CardAction> cardButtons = new List<CardAction>();
            CardAction Button1 = new CardAction()
            {
                Value = "ok you are interested in ux/ui",
                Type = "postBack",
                Title = "UX / UI Designer"
            };
            CardAction Button2 = new CardAction()
            {
                Value = "ok you are interested in graphic",
                Type = "postBack",
                Title = "Graphic Designer",
            };

            cardButtons.Add(Button1);
            cardButtons.Add(Button2);

            HeroCard jobCard = new HeroCard()
            {
                Buttons = cardButtons,
            };

            Attachment jobAttachment = jobCard.ToAttachment();
            reply.Attachments.Add(jobAttachment);
            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
        }

        private void select_development(Activity reply)
        {
            reply.Attachments = new List<Attachment>();
            List<CardAction> cardButtons = new List<CardAction>();
            CardAction Button1 = new CardAction()
            {
                Value = "ok you are interested in fullstack",
                Type = "postBack",
                Title = "Full Stack Web Developer"
            };
            CardAction Button2 = new CardAction()
            {
                Value = "ok you are interested in frontend",
                Type = "postBack",
                Title = "Front End Web Developer",
            };
            CardAction Button3 = new CardAction()
            {
                Value = "ok you are interested in application",
                Type = "postBack",
                Title = "Application Developer",
            };
            CardAction Button4 = new CardAction()
            {
                Value = "ok you are interested in mobile",
                Type = "postBack",
                Title = "Mobile Developer",
            };
            CardAction Button5 = new CardAction()
            {
                Value = "ok you are interested in cloud",
                Type = "postBack",
                Title = "Cloud Developer",
            };
            cardButtons.Add(Button1);
            cardButtons.Add(Button2);
            cardButtons.Add(Button3);
            cardButtons.Add(Button4);
            cardButtons.Add(Button5);
            HeroCard jobCard = new HeroCard()
            {
                Buttons = cardButtons,
            };

            Attachment jobAttachment = jobCard.ToAttachment();
            reply.Attachments.Add(jobAttachment);
            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
        }

        private void select_marketing(Activity reply)
        {
            reply.Attachments = new List<Attachment>();
            List<CardAction> cardButtons = new List<CardAction>();
            CardAction Button1 = new CardAction()
            {
                Value = "ok you are interested in digital",
                Type = "postBack",
                Title = "Digital Marketer"
            };
            CardAction Button2 = new CardAction()
            {
                Value = "ok you are interested in sales",
                Type = "postBack",
                Title = "Sales Representative",
            };

            cardButtons.Add(Button1);
            cardButtons.Add(Button2);

            HeroCard jobCard = new HeroCard()
            {
                Buttons = cardButtons,
            };

            Attachment jobAttachment = jobCard.ToAttachment();
            reply.Attachments.Add(jobAttachment);
            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
        }





        //private void select_design(Activity reply)
        //{
        //    reply.Attachments = new List<Attachment>();
        //    List<CardAction> cardButtons = new List<CardAction>();
        //    CardAction Button1 = new CardAction()
        //    {
        //        Value = "ok you are designer of ux/ui",
        //        Type = "postBack",
        //        Title = "UX / UI Designer"
        //    };
        //    CardAction Button2 = new CardAction()
        //    {
        //        Value = "ok you are designer of graphic",
        //        Type = "postBack",
        //        Title = "Graphic Designer",
        //    };
        
        //    cardButtons.Add(Button1);
        //    cardButtons.Add(Button2);
        
        //    HeroCard jobCard = new HeroCard()
        //    {
        //        Buttons = cardButtons,
        //    };

        //    Attachment jobAttachment = jobCard.ToAttachment();
        //    reply.Attachments.Add(jobAttachment);
        //    reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
        //}

        //private void select_development(Activity reply)
        //{
        //    reply.Attachments = new List<Attachment>();
        //    List<CardAction> cardButtons = new List<CardAction>();
        //    CardAction Button1 = new CardAction()
        //    {
        //        Value = "ok you are developer of fullstack",
        //        Type = "postBack",
        //        Title = "Full Stack Web Developer"
        //    };
        //    CardAction Button2 = new CardAction()
        //    {
        //        Value = "ok you are developer of frontend",
        //        Type = "postBack",
        //        Title = "Front End Web Developer",
        //    };
        //    CardAction Button3 = new CardAction()
        //    {
        //        Value = "ok you are developer of application",
        //        Type = "postBack",
        //        Title = "Application Developer",
        //    };
        //    CardAction Button4 = new CardAction()
        //    {
        //        Value = "ok you are developer of mobile",
        //        Type = "postBack",
        //        Title = "Mobile Developer",
        //    };
        //    CardAction Button5 = new CardAction()
        //    {
        //        Value = "ok you are developer of cloud",
        //        Type = "postBack",
        //        Title = "Cloud Developer",
        //    };
        //    cardButtons.Add(Button1);
        //    cardButtons.Add(Button2);
        //    cardButtons.Add(Button3);
        //    cardButtons.Add(Button4);
        //    cardButtons.Add(Button5);
        //    HeroCard jobCard = new HeroCard()
        //    {
        //        Buttons = cardButtons,
        //    };

        //    Attachment jobAttachment = jobCard.ToAttachment();
        //    reply.Attachments.Add(jobAttachment);
        //    reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
        //}

        //private void select_marketing(Activity reply)
        //{
        //    reply.Attachments = new List<Attachment>();
        //    List<CardAction> cardButtons = new List<CardAction>();
        //    CardAction Button1 = new CardAction()
        //    {
        //        Value = "ok you are marketerofdigital",
        //        Type = "postBack",
        //        Title = "Digital Marketer"
        //    };
        //    CardAction Button2 = new CardAction()
        //    {
        //        Value = "ok you are marketerofsales",
        //        Type = "postBack",
        //        Title = "Sales Representative",
        //    };

        //    cardButtons.Add(Button1);
        //    cardButtons.Add(Button2);

        //    HeroCard jobCard = new HeroCard()
        //    {
        //        Buttons = cardButtons,
        //    };

        //    Attachment jobAttachment = jobCard.ToAttachment();
        //    reply.Attachments.Add(jobAttachment);
        //    reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
        //}

        private void design_types(Activity reply,string position)
        {       
            switch (position)
            {
                case "ux/ui":
                    uxui_designer(reply);
                    break;
                case "graphic":
                    graphic_designer(reply);
                    break;
            }
        }

        private void development_types(Activity reply,string position)
        {
            switch (position)
            {
                case "fullstack":
                    fullstack_developer(reply);
                    break;
                case "frontend":
                    frontend_developer(reply);
                    break;
                case "application":
                    application_developer(reply);
                    break;
                case "mobile":
                    mobile_developer(reply);
                    break;
                case "cloud":
                    cloud_developer(reply);
                    break;
            }
        }

        private void marketing_types(Activity reply, LuisResponse luis)
        {
            var enty = luis.entities[0].entity.ToLower();
            enty = enty.Substring(10);
            switch (enty)
            {
                case "digital":
                    digital_marketer(reply, luis);
                    break;
                case "sales":
                    sales_representator(reply, luis);
                    break;
            }

        }

        private string module_types(Activity reply, LuisResponse luis, string mesj)
        {
            var enty = luis.entities[0].entity.ToLower();
            enty = enty.Substring(9);
            switch (enty)
            {
                case "uxui":
                    mesj = uxui_module(reply, luis, mesj);
                    break;
                case "graphic":
                    mesj = graphic_module(reply, luis, mesj);
                    break;
                case "fullstack":
                    mesj = fullstack_module(reply, luis, mesj);
                    break;
                case "frontend":
                    mesj = frontend_module(reply, luis, mesj);
                    break;
                case "application":
                    mesj = application_module(reply, luis, mesj);
                    break;
                case "mobile":
                    mesj = mobile_module(reply, luis, mesj);
                    break;
                case "cloud":
                    mesj = cloud_module(reply, luis, mesj);
                    break;
            }

            return mesj;
        }

        private void uxui_designer(Activity reply)
        {
            reply.Attachments = new List<Attachment>();
            List<CardAction> cardButtons = new List<CardAction>();
            CardAction Button1 = new CardAction()
            {
                Value = "ok you want to complete module1ofuxui",
                Type = "postBack",
                Title = "Layout and Composition"
            };
            CardAction Button2 = new CardAction()
            {
                Value = "ok you want to complete module2ofuxui",
                Type = "postBack",
                Title = "Typography",
            };
            CardAction Button3 = new CardAction()
            {
                Value = "ok you want to complete module3ofuxui",
                Type = "postBack",
                Title = "Logo Design",
            };
            CardAction Button4 = new CardAction()
            {
                Value = "ok you want to complete module4ofuxui",
                Type = "postBack",
                Title = "Design Tools",
            };
           
            cardButtons.Add(Button1);
            cardButtons.Add(Button2);
            cardButtons.Add(Button3);
            cardButtons.Add(Button4);
            HeroCard jobCard = new HeroCard()
            {
                Buttons = cardButtons,
            };

            Attachment jobAttachment = jobCard.ToAttachment();
            reply.Attachments.Add(jobAttachment);
            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
        }

        private void graphic_designer(Activity reply)
        {
            reply.Attachments = new List<Attachment>();
            List<CardAction> cardButtons = new List<CardAction>();
            CardAction Button1 = new CardAction()
            {
                Value = "ok you want to complete module1ofgraphic",
                Type = "postBack",
                Title = "Layout and Composition"
            };
            CardAction Button2 = new CardAction()
            {
                Value = "ok you want to complete module2ofgraphic",
                Type = "postBack",
                Title = "Typography",
            };
            CardAction Button3 = new CardAction()
            {
                Value = "ok you want to complete module3ofgraphic",
                Type = "postBack",
                Title = "Logo Design",
            };
            CardAction Button4 = new CardAction()
            {
                Value = "ok you want to complete module4ofgraphic",
                Type = "postBack",
                Title = "Design Tools",
            };

            cardButtons.Add(Button1);
            cardButtons.Add(Button2);
            cardButtons.Add(Button3);
            cardButtons.Add(Button4);
            HeroCard jobCard = new HeroCard()
            {
                Buttons = cardButtons,
            };

            Attachment jobAttachment = jobCard.ToAttachment();
            reply.Attachments.Add(jobAttachment);
            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
        }

        private void fullstack_developer(Activity reply)
        {
            reply.Attachments = new List<Attachment>();
            List<CardAction> cardButtons = new List<CardAction>();
            CardAction Button1 = new CardAction()
            {
                Value = "ok you want to complete module1offullstack",
                Type = "postBack",
                Title = "Web Development Foundations"
            };
            CardAction Button2 = new CardAction()
            {
                Value = "ok you want to complete module2offullstack",
                Type = "postBack",
                Title = "Programing Fundamentals",
            };
            CardAction Button3 = new CardAction()
            {
                Value = "ok you want to complete module3offullstack",
                Type = "postBack",
                Title = "Essential Frontend Languages",
            };
            CardAction Button4 = new CardAction()
            {
                Value = "ok you want to complete module4offullstack",
                Type = "postBack",
                Title = "Database Foundations",
            };
            CardAction Button5 = new CardAction()
            {
                Value = "ok you want to complete module5offullstack",
                Type = "postBack",
                Title = "Essential Backend Languages",
            };
            CardAction Button6 = new CardAction()
            {
                Value = "ok you want to complete module6offullstack",
                Type = "postBack",
                Title = "Security Foundations",
            };
            CardAction Button7 = new CardAction()
            {
                Value = "ok you want to complete module7offullstack",
                Type = "postBack",
                Title = "Web Projects Workflows",
            };
            CardAction Button8 = new CardAction()
            {
                Value = "ok you want to complete module8offullstack",
                Type = "postBack",
                Title = "Fullstack Frameworks",
            };
            cardButtons.Add(Button1);
            cardButtons.Add(Button2);
            cardButtons.Add(Button3);
            cardButtons.Add(Button4);
            cardButtons.Add(Button5);
            cardButtons.Add(Button6);
            cardButtons.Add(Button7);
            cardButtons.Add(Button8);
            HeroCard jobCard = new HeroCard()
            {
                Buttons = cardButtons,
            };

            Attachment jobAttachment = jobCard.ToAttachment();
            reply.Attachments.Add(jobAttachment);
            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
        }

        private void frontend_developer(Activity reply)
        {
            reply.Attachments = new List<Attachment>();
            List<CardAction> cardButtons = new List<CardAction>();
            CardAction Button1 = new CardAction()
            {
                Value = "ok you want to complete module1offrontend",
                Type = "postBack",
                Title = "Web Development Foundations"
            };
            CardAction Button2 = new CardAction()
            {
                Value = "ok you want to complete module2offrontend",
                Type = "postBack",
                Title = "Programing Fundamentals",
            };
            CardAction Button3 = new CardAction()
            {
                Value = "ok you want to complete module3offrontend",
                Type = "postBack",
                Title = "User Experience for Web Designers",
            };
            CardAction Button4 = new CardAction()
            {
                Value = "ok you want to complete module4offrontend",
                Type = "postBack",
                Title = "UX Foundations Accessibility",
            };
            CardAction Button5 = new CardAction()
            {
                Value = "ok you want to complete module5offrontend",
                Type = "postBack",
                Title = "Responsive Design",
            };
            CardAction Button6 = new CardAction()
            {
                Value = "ok you want to complete module6offrontend",
                Type = "postBack",
                Title = "Framework & Tools",
            };
            cardButtons.Add(Button1);
            cardButtons.Add(Button2);
            cardButtons.Add(Button3);
            cardButtons.Add(Button4);
            cardButtons.Add(Button5);
            cardButtons.Add(Button6);
            HeroCard jobCard = new HeroCard()
            {
                Buttons = cardButtons,
            };

            Attachment jobAttachment = jobCard.ToAttachment();
            reply.Attachments.Add(jobAttachment);
            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
        }

        private void application_developer(Activity reply)
        {
            reply.Attachments = new List<Attachment>();
            List<CardAction> cardButtons = new List<CardAction>();
            CardAction Button1 = new CardAction()
            {
                Value = "ok you want to complete module1ofapplication",
                Type = "postBack",
                Title = "Java"
            };
            CardAction Button2 = new CardAction()
            {
                Value = "ok you want to complete module2ofapplication",
                Type = "postBack",
                Title = "C++",
            };
            CardAction Button3 = new CardAction()
            {
                Value = "ok you want to complete module3ofapplication",
                Type = "postBack",
                Title = "Python",
            };    
            cardButtons.Add(Button1);
            cardButtons.Add(Button2);
            cardButtons.Add(Button3);
            HeroCard jobCard = new HeroCard()
            {
                Buttons = cardButtons,
            };
            Attachment jobAttachment = jobCard.ToAttachment();
            reply.Attachments.Add(jobAttachment);
            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
        }

        private void mobile_developer(Activity reply)
        {
            reply.Attachments = new List<Attachment>();
            List<CardAction> cardButtons = new List<CardAction>();
            CardAction Button1 = new CardAction()
            {
                Value = "ok you want to complete module1ofmobile",
                Type = "postBack",
                Title = "Android"
            };
            CardAction Button2 = new CardAction()
            {
                Value = "ok you want to complete module2ofmobile",
                Type = "postBack",
                Title = "iOS",
            };
            CardAction Button3 = new CardAction()
            {
                Value = "ok you want to complete module3ofmobile",
                Type = "postBack",
                Title = "Cross-Platform",
            };
       
            cardButtons.Add(Button1);
            cardButtons.Add(Button2);
            cardButtons.Add(Button3);
            HeroCard jobCard = new HeroCard()
            {
               
                Buttons = cardButtons,
            };

            Attachment jobAttachment = jobCard.ToAttachment();
            reply.Attachments.Add(jobAttachment);
            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
        }

        private void cloud_developer(Activity reply)
        {
            reply.Attachments = new List<Attachment>();
            List<CardAction> cardButtons = new List<CardAction>();
            CardAction Button1 = new CardAction()
            {
                Value = "ok you want to complete module1ofcloud",
                Type = "postBack",
                Title = "Web Development Foundations"
            };
            CardAction Button2 = new CardAction()
            {
                Value = "ok you want to complete module2ofcloud",
                Type = "postBack",
                Title = "Programing Fundamentals",
            };
            CardAction Button3 = new CardAction()
            {
                Value = "ok you want to complete module3ofcloud",
                Type = "postBack",
                Title = "Essential Frontend Languages",
            };
        
            cardButtons.Add(Button1);
            cardButtons.Add(Button2);
            cardButtons.Add(Button3);
                   
            HeroCard jobCard = new HeroCard()
            {
                Buttons = cardButtons,
            };

            Attachment jobAttachment = jobCard.ToAttachment();
            reply.Attachments.Add(jobAttachment);
            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
        }

        private void digital_marketer(Activity reply, LuisResponse luis)
        {
            reply.Attachments = new List<Attachment>();
            List<CardAction> cardButtons = new List<CardAction>();
            CardAction Button1 = new CardAction()
            {
                Value = "you selected the test Dig Online Marketing Foundations",
                Type = "postBack",
                Title = "Online Marketing Foundations"
            };
            CardAction Button2 = new CardAction()
            {
                Value = "you selected the test Dig Seo Foundations",
                Type = "postBack",
                Title = "SEO Foundations",
            };
            CardAction Button3 = new CardAction()
            {
                Value = "you selected the test Dig Google Analytics",
                Type = "postBack",
                Title = "Google Analytics",
            };
            CardAction Button4 = new CardAction()
            {
                Value = "you selected the test Dig Content Marketing Fundamentals",
                Type = "postBack",
                Title = "Content Marketing Fundamentals",
            };
            CardAction Button5 = new CardAction()
            {
                Value = "you selected the test Dig Mobile Marketing Foundations",
                Type = "postBack",
                Title = "Mobile Marketing Foundations",
            };
            CardAction Button6 = new CardAction()
            {
                Value = "you selected the test Dig Lead Generation Foundations",
                Type = "postBack",
                Title = "Lead Generation Foundations",
            };
            CardAction Button7 = new CardAction()
            {
                Value = "you selected the test Dig Growth Hacking",
                Type = "postBack",
                Title = "Growth Hacking",
            };
            CardAction Button8 = new CardAction()
            {
                Value = "you selected the test Dig Online Marketing Plan",
                Type = "postBack",
                Title = "Online Marketing Plan",
            };
            cardButtons.Add(Button1);
            cardButtons.Add(Button2);
            cardButtons.Add(Button3);
            cardButtons.Add(Button4);
            cardButtons.Add(Button5);
            cardButtons.Add(Button6);
            cardButtons.Add(Button7);
            cardButtons.Add(Button8);
            HeroCard jobCard = new HeroCard()
            {
                Buttons = cardButtons,
            };

            Attachment jobAttachment = jobCard.ToAttachment();
            reply.Attachments.Add(jobAttachment);
            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
        }

        private void sales_representator(Activity reply, LuisResponse luis)
        {
            reply.Attachments = new List<Attachment>();
            List<CardAction> cardButtons = new List<CardAction>();
            CardAction Button1 = new CardAction()
            {
                Value = "you selected the test Sal Sales Career",
                Type = "postBack",
                Title = "Sales Career"
            };
            CardAction Button2 = new CardAction()
            {
                Value = "you selected the test Sal Effective Listening",
                Type = "postBack",
                Title = "Effective Listening",
            };
            CardAction Button3 = new CardAction()
            {
                Value = "you selected the test Sal Assertiveness",
                Type = "postBack",
                Title = "Assertiveness",
            };
            CardAction Button4 = new CardAction()
            {
                Value = "you selected the test Sal Resilience",
                Type = "postBack",
                Title = "Resilience",
            };
            CardAction Button5 = new CardAction()
            {
                Value = "you selected the test Sal Sales Foundations",
                Type = "postBack",
                Title = "Sales Foundations",
            };
            CardAction Button6 = new CardAction()
            {
                Value = "you selected the test Sal Asking Questions",
                Type = "postBack",
                Title = "Asking Questions",
            };
            CardAction Button7 = new CardAction()
            {
                Value = "you selected the test Sal Creating Customer Value",
                Type = "postBack",
                Title = "Creating Customer Value",
            };
            CardAction Button8 = new CardAction()
            {
                Value = "you selected the test Sal Sales Negotiation",
                Type = "postBack",
                Title = "Sales Negotiation",
            };
            cardButtons.Add(Button1);
            cardButtons.Add(Button2);
            cardButtons.Add(Button3);
            cardButtons.Add(Button4);
            cardButtons.Add(Button5);
            cardButtons.Add(Button6);
            cardButtons.Add(Button7);
            cardButtons.Add(Button8);
            HeroCard jobCard = new HeroCard()
            {
                Buttons = cardButtons,
            };

            Attachment jobAttachment = jobCard.ToAttachment();
            reply.Attachments.Add(jobAttachment);
            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
        }

        private string retrieve_modulename(string no)
        {
            var user = JsonConvert.DeserializeObject<MongoData>(MongoUser.exist_user(id).ToJson());
            data.Add(user.professional.position_type);
            var test = JsonConvert.DeserializeObject<Structure>(MongoUser.test_name("123").ToJson());
            var mod = ((test.teststructure.courses.Where(x => x.course_name.ToLower() == user.professional.occupation_type.ToLower()).SingleOrDefault())
                      .Tracks.Where(x => x.track_name.ToLower() == user.professional.position_type.ToLower()).SingleOrDefault())
                      .Modules.Where(z => z.module_no.ToLower() == no.ToLower()).SingleOrDefault();
            return mod.module_name;
        }

        private string uxui_module(Activity reply, LuisResponse luis, string mesj)
        {

            var ent = luis.entities[0].entity.ToLower();
            ent = ent.Substring(0, 7);
            data.Add(ent.Substring(6));
          
            switch (ent)
            {
                case "module1":
                    reply.Text = retrieve_modulename(ent.Substring(6));
                    mesj = giving_test(reply,mesj);//new
                    reply.Text = col.ElementAt(84);
                          
                    break;

                case "module2":
                    reply.Text = retrieve_modulename(ent.Substring(6));
                    mesj = giving_test(reply, mesj);
                    reply.Text = col.ElementAt(85);
                    break;

                case "module3":
                    reply.Text = retrieve_modulename(ent.Substring(6));
                    mesj = giving_test(reply, mesj);
                    reply.Text = col.ElementAt(86);
                    break;

                case "module4": // change here
                    mesj = col.ElementAt(87);
                    reply.Text = retrieve_modulename(ent.Substring(6));
                    data.Add(reply.Text);
                    designtools_test(reply,1);
                    reply.Text = null;
                    break;
                                  
            }
     
            return mesj;
        }

        private string graphic_module(Activity reply, LuisResponse luis, string mesj)
        {

            var ent = luis.entities[0].entity.ToLower();
            ent = ent.Substring(0, 7);
            data.Add(ent.Substring(6));
            switch (ent)
            {
                case "module1":
                    reply.Text = retrieve_modulename(ent.Substring(6));
                    mesj = giving_test(reply, mesj);
                    reply.Text = col.ElementAt(84);
                    break;

                case "module2":
                    reply.Text = retrieve_modulename(ent.Substring(6));
                    mesj = giving_test(reply, mesj);
                    reply.Text = col.ElementAt(85);
                    break;

                case "module3":
                    reply.Text = retrieve_modulename(ent.Substring(6));
                    mesj = giving_test(reply, mesj);
                    reply.Text = col.ElementAt(86);
                    break;

                case "module4":
                    mesj = col.ElementAt(87);
                    reply.Text = retrieve_modulename(ent.Substring(6));
                    data.Add(reply.Text);
                    designtools_test(reply,2);
                    reply.Text = null;                                      
                    break;

            }
        
            return mesj;
        }

        private string fullstack_module(Activity reply,LuisResponse luis,string mesj)
        {
           
            var ent = luis.entities[0].entity.ToLower();
            ent = ent.Substring(0,7);
            data.Add(ent.Substring(6));
            switch (ent)
            {
                case "module1":
                    mesj = col.ElementAt(50);
                    reply.Text = retrieve_modulename(ent.Substring(6));
                    data.Add(reply.Text);
                    webdevelopment_test(reply,1);
                    reply.Text = null;
                    break;

                case "module2":
                    reply.Text = retrieve_modulename(ent.Substring(6));
                    mesj = giving_test(reply, mesj);
                    reply.Text = col.ElementAt(51);
                    break;

                case "module3":
                    mesj = col.ElementAt(52);
                    reply.Text = retrieve_modulename(ent.Substring(6));
                    data.Add(reply.Text);
                    frontend_test(reply);
                    reply.Text = null;
                    break;

                case "module4":
                    reply.Text = retrieve_modulename(ent.Substring(6));
                    mesj = giving_test(reply, mesj);
                    reply.Text = col.ElementAt(53);
                    break;

                case "module5":
                    mesj = col.ElementAt(54);
                    reply.Text = retrieve_modulename(ent.Substring(6));
                    data.Add(reply.Text);
                    backend_test(reply);
                    reply.Text = null;
                    break;

                case "module6":
                    reply.Text = retrieve_modulename(ent.Substring(6));
                    mesj = giving_test(reply, mesj);
                    reply.Text = col.ElementAt(55);
                    break;

                case "module7":
                    mesj = col.ElementAt(56);
                    reply.Text = retrieve_modulename(ent.Substring(6));
                    data.Add(reply.Text);
                    projects_test(reply);
                    reply.Text = null;
                    break;

                case "module8":
                    reply.Text = retrieve_modulename(ent.Substring(6));
                    mesj = giving_test(reply, mesj);
                    reply.Text = col.ElementAt(57);
                    break;

            }
           
            return mesj;
        }

        private string frontend_module(Activity reply, LuisResponse luis, string mesj)
        {

            var ent = luis.entities[0].entity.ToLower();
            ent = ent.Substring(0, 7);
            data.Add(ent.Substring(6));
            switch (ent)
            {
                case "module1":
                    mesj = col.ElementAt(50);
                    reply.Text = retrieve_modulename(ent.Substring(6));
                    data.Add(reply.Text);
                    webdevelopment_test(reply,2);
                    reply.Text = null;
                    break;

                case "module2":
                    reply.Text = retrieve_modulename(ent.Substring(6));
                    mesj = giving_test(reply, mesj);
                    reply.Text = col.ElementAt(51);
                    break;

                case "module3":
                    reply.Text = retrieve_modulename(ent.Substring(6));
                    mesj = giving_test(reply, mesj);
                    reply.Text = col.ElementAt(58);
                    break;

                case "module4":
                    reply.Text = retrieve_modulename(ent.Substring(6));
                    mesj = giving_test(reply, mesj);
                    reply.Text = col.ElementAt(59);
                    break;

                case "module5":
                    reply.Text = retrieve_modulename(ent.Substring(6));
                    mesj = giving_test(reply, mesj);
                    reply.Text = col.ElementAt(60);
                    break;

                case "module6":
                    mesj = col.ElementAt(61);
                    reply.Text = retrieve_modulename(ent.Substring(6));
                    data.Add(reply.Text);
                    frameworktools_test(reply);
                    reply.Text = null;
                    break;
            }
            return mesj;
        }
            
        private string application_module(Activity reply, LuisResponse luis, string mesj)
        {
            var ent = luis.entities[0].entity.ToLower();
            ent = ent.Substring(0, 7);
            data.Add(ent.Substring(6));
            switch (ent)
            {
                case "module1":
                    mesj = col.ElementAt(50);
                    reply.Text = retrieve_modulename(ent.Substring(6));
                    data.Add(reply.Text);
                    java_test(reply);
                    reply.Text = null;
                    break;

                case "module2":
                    mesj = col.ElementAt(51);
                    reply.Text = retrieve_modulename(ent.Substring(6));
                    data.Add(reply.Text);
                    csharp_test(reply);
                    reply.Text = null;
                    break;

                case "module3":
                    mesj = col.ElementAt(52);
                    reply.Text = retrieve_modulename(ent.Substring(6));
                    data.Add(reply.Text);
                    python_test(reply);
                    reply.Text = null;
                    break;

            }
            return mesj;
        }

        private string mobile_module(Activity reply, LuisResponse luis, string mesj)
        {
            var ent = luis.entities[0].entity.ToLower();
            ent = ent.Substring(0, 7);
            data.Add(ent.Substring(6));
            switch (ent)
            {
                case "module1":
                    mesj = col.ElementAt(50);
                    reply.Text = retrieve_modulename(ent.Substring(6));
                    data.Add(reply.Text);
                    android_test(reply);
                    reply.Text = null;
                    break;

                case "module2":
                    mesj = col.ElementAt(51);
                    reply.Text = retrieve_modulename(ent.Substring(6));
                    data.Add(reply.Text);
                    ios_test(reply);
                    reply.Text = null;
                    break;

                case "module3":
                    mesj = col.ElementAt(51);
                    reply.Text = retrieve_modulename(ent.Substring(6));
                    data.Add(reply.Text);
                    cross_test(reply);
                    reply.Text = null;
                    break;            
            }
            return mesj;
        }

        private string cloud_module(Activity reply, LuisResponse luis, string mesj)
        {

            var ent = luis.entities[0].entity.ToLower();
            ent = ent.Substring(0, 7);
            data.Add(ent.Substring(6));
            switch (ent)
            {
                case "module1":
                    mesj = col.ElementAt(50);
                    webdevelopment_test(reply,1);

                    break;

                case "module2":
                    reply.Text = retrieve_modulename(ent.Substring(6));
                    giving_test(reply, mesj);
                    reply.Text = col.ElementAt(51);
                    break;

                case "module3":
                    mesj = col.ElementAt(52);
                    frontend_test(reply);

                    break;

                case "module4":
                    reply.Text = retrieve_modulename(ent.Substring(6));
                    giving_test(reply, mesj);
                    reply.Text = col.ElementAt(53);
                    break;

                case "module5":
                    mesj = col.ElementAt(54);
                    backend_test(reply);
                    break;

                case "module6":
                    reply.Text = retrieve_modulename(ent.Substring(6));
                    giving_test(reply, mesj);
                    reply.Text = col.ElementAt(55);
                    break;

                case "module7":
                    mesj = col.ElementAt(56);
                    projects_test(reply);
                    break;

                case "module8":
                    reply.Text = retrieve_modulename(ent.Substring(6));
                    giving_test(reply, mesj);
                    reply.Text = col.ElementAt(57);
                    break;

            }
            return mesj;
        }

        private void designtools_test(Activity reply,int choice)
        {
            string type = null;
            if (choice == 1)
            {
                type = "UXUI";
            }
            else
            {
                type = "Grap";
            }
            reply.Attachments = new List<Attachment>();
            List<CardAction> cardButtons = new List<CardAction>();
            CardAction Button1 = new CardAction()
            {
                Value = "you selected the test " + type + " Photoshop",
                Type = "postBack",
                Title = "Photoshop"
            };
            CardAction Button2 = new CardAction()
            {
                Value = "you selected the test " + type + " InDesign",
                Type = "postBack",
                Title = "InDesign",
            };
            cardButtons.Add(Button1);
            cardButtons.Add(Button2);
            HeroCard jobCard = new HeroCard()
            {
                Buttons = cardButtons,
            };

            Attachment jobAttachment = jobCard.ToAttachment();
            reply.Attachments.Add(jobAttachment);
            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
        }

        private void webdevelopment_test(Activity reply,int choice)
        {
            string type = null;
            if (choice == 1)
            {
                type = "Full";
            }
            else
            {
                type = "Fro";
            }
            reply.Attachments = new List<Attachment>();
            List<CardAction> cardButtons = new List<CardAction>();
            CardAction Button1 = new CardAction()
            {
                Value = "you selected the test " + type + " Frontend",
                Type = "postBack",
                Title = "Frontend"
            };
            CardAction Button2 = new CardAction()
            {
                Value = "you selected the test " + type + " Backend",
                Type = "postBack",
                Title = "Backend",
            };
            CardAction Button3 = new CardAction()
            {
                Value = "you selected the test " + type + " Fullstack",
                Type = "postBack",
                Title = "Fullstack",
            };
            cardButtons.Add(Button1);
            cardButtons.Add(Button2);
            cardButtons.Add(Button3);
            HeroCard jobCard = new HeroCard()
            {
                Buttons = cardButtons,
            };

            Attachment jobAttachment = jobCard.ToAttachment();
            reply.Attachments.Add(jobAttachment);
            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
        }

        private void frontend_test(Activity reply)
        {
            reply.Attachments = new List<Attachment>();
            List<CardAction> cardButtons = new List<CardAction>();
            CardAction Button1 = new CardAction()
            {
                Value = "you selected the test Full HTML",
                Type = "postBack",
                Title = "HTML"
            };
            CardAction Button2 = new CardAction()
            {
                Value = "you selected the test Full CSS",
                Type = "postBack",
                Title = "CSS",
            };
            CardAction Button3 = new CardAction()
            {
                Value = "you selected the test Full Javascript",
                Type = "postBack",
                Title = "Javascript",
            };
            cardButtons.Add(Button1);
            cardButtons.Add(Button2);
            cardButtons.Add(Button3);
            HeroCard jobCard = new HeroCard()
            {
                Buttons = cardButtons,
            };

            Attachment jobAttachment = jobCard.ToAttachment();
            reply.Attachments.Add(jobAttachment);
            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
        }

        private void backend_test(Activity reply)
        {
            
            reply.Attachments = new List<Attachment>();
            List<CardAction> cardButtons = new List<CardAction>();
            CardAction Button1 = new CardAction()
            {
                Value = "you selected the test Full PHP",
                Type = "postBack",
                Title = "PHP"
            };
            CardAction Button2 = new CardAction()
            {
                Value = "you selected the test Full Python",
                Type = "postBack",
                Title = "Python",
            };
            CardAction Button3 = new CardAction()
            {
                Value = "you selected the test Full SQL",
                Type = "postBack",
                Title = "SQL",
            };
            cardButtons.Add(Button1);
            cardButtons.Add(Button2);
            cardButtons.Add(Button3);
            HeroCard jobCard = new HeroCard()
            {
                Buttons = cardButtons,
            };

            Attachment jobAttachment = jobCard.ToAttachment();
            reply.Attachments.Add(jobAttachment);
            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
        }

        private void projects_test(Activity reply)
        {
            reply.Attachments = new List<Attachment>();
            List<CardAction> cardButtons = new List<CardAction>();
            CardAction Button1 = new CardAction()
            {
                Value = "you selected the test Full Gulp.js",
                Type = "postBack",
                Title = "Gulp.js"
            };
            CardAction Button2 = new CardAction()
            {
                Value = "you selected the test Full Git",
                Type = "postBack",
                Title = "Git",
            };
            CardAction Button3 = new CardAction()
            {
                Value = "you selected the test Full Browserify",
                Type = "postBack",
                Title = "Browserify",
            };
            cardButtons.Add(Button1);
            cardButtons.Add(Button2);
            cardButtons.Add(Button3);
            HeroCard jobCard = new HeroCard()
            {
                Buttons = cardButtons,
            };

            Attachment jobAttachment = jobCard.ToAttachment();
            reply.Attachments.Add(jobAttachment);
            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
        }

        private void frameworktools_test(Activity reply)
        {
            reply.Attachments = new List<Attachment>();
            List<CardAction> cardButtons = new List<CardAction>();
            CardAction Button1 = new CardAction()
            {
                Value = "you selected the test Fro Bootstrap",
                Type = "postBack",
                Title = "Bootstrap 3"
            };
            CardAction Button2 = new CardAction()
            {
                Value = "you selected the test Fro React.js",
                Type = "postBack",
                Title = "React.js",
            };
            CardAction Button3 = new CardAction()
            {
                Value = "you selected the test Fro Sass",
                Type = "postBack",
                Title = "Sass",
            };
            cardButtons.Add(Button1);
            cardButtons.Add(Button2);
            cardButtons.Add(Button3);
            HeroCard jobCard = new HeroCard()
            {
                Buttons = cardButtons,
            };

            Attachment jobAttachment = jobCard.ToAttachment();
            reply.Attachments.Add(jobAttachment);
            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
        }

        private void java_test(Activity reply)
        {
            reply.Attachments = new List<Attachment>();
            List<CardAction> cardButtons = new List<CardAction>();
            CardAction Button1 = new CardAction()
            {
                Value = "you selected the test App Object Oriented Design62",
                Type = "postBack",
                Title = "Object-Oriented Design"
            };
            CardAction Button2 = new CardAction()
            {
                Value = "you selected the test App Java Essential Training63",
                Type = "postBack",
                Title = "Java & Essential Training",
            };
            CardAction Button3 = new CardAction()
            {
                Value = "you selected the test App Code Clinic Java64",
                Type = "postBack",
                Title = "Code Clinic:Java",
            };
            CardAction Button4 = new CardAction()
            {
                Value = "you selected the test App Design Patterns65",
                Type = "postBack",
                Title = "Design Patterns",
            };
            CardAction Button5 = new CardAction()
            {
                Value = "you selected the test App Advanced Java Programming66",
                Type = "postBack",
                Title = "Advanced Java Programming",
            };
            CardAction Button6 = new CardAction()
            {
                Value = "you selected the test App Java Database Integration67",
                Type = "postBack",
                Title = "Java:Database Integration JDBC",
            };
            cardButtons.Add(Button1);
            cardButtons.Add(Button2);
            cardButtons.Add(Button3);
            cardButtons.Add(Button4);
            cardButtons.Add(Button5);
            cardButtons.Add(Button6);
            HeroCard jobCard = new HeroCard()
            {
                Buttons = cardButtons,
            };

            Attachment jobAttachment = jobCard.ToAttachment();
            reply.Attachments.Add(jobAttachment);
            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
        }

        private void csharp_test(Activity reply)
        {
            reply.Attachments = new List<Attachment>();
            List<CardAction> cardButtons = new List<CardAction>();
            CardAction Button1 = new CardAction()
            {
                Value = "you selected the test Cpp Essential104",
                Type = "postBack",
                Title = "Cpp Essential"
            };
            CardAction Button2 = new CardAction()
            {
                Value = "you selected the test Cpp Code Clinic105",
                Type = "postBack",
                Title = "Code Clinic",
            };
            CardAction Button3 = new CardAction()
            {
                Value = "you selected the test Cpp Move Semantics106",
                Type = "postBack",
                Title = "Move Semantics",
            };
            CardAction Button4 = new CardAction()
            {
                Value = "you selected the test Cpp Smart Pointers107",
                Type = "postBack",
                Title = "Smart Pointers",
            };
            CardAction Button5 = new CardAction()
            {
                Value = "you selected the test Cpp Building a String Library108",
                Type = "postBack",
                Title = "Building a String Library",
            };
           
            cardButtons.Add(Button1);
            cardButtons.Add(Button2);
            cardButtons.Add(Button3);
            cardButtons.Add(Button4);
            cardButtons.Add(Button5);
            HeroCard jobCard = new HeroCard()
            {
                Buttons = cardButtons,
            };

            Attachment jobAttachment = jobCard.ToAttachment();
            reply.Attachments.Add(jobAttachment);
            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
        }

        private void python_test(Activity reply)
        {
            reply.Attachments = new List<Attachment>();
            List<CardAction> cardButtons = new List<CardAction>();
            CardAction Button1 = new CardAction()
            {
                Value = "you selected the test Pyt Programming Foundations109",
                Type = "postBack",
                Title = "Programming Foundations"
            };
            CardAction Button2 = new CardAction()
            {
                Value = "you selected the test Pyt Essentials110",
                Type = "postBack",
                Title = "Essentials",
            };
            CardAction Button3 = new CardAction()
            {
                Value = "you selected the test Pyt Python GUI111",
                Type = "postBack",
                Title = "Python GUI",
            };
            CardAction Button4 = new CardAction()
            {
                Value = "you selected the test Pyt Code Clinic112",
                Type = "postBack",
                Title = "Code Clinic",
            };
            CardAction Button5 = new CardAction()
            {
                Value = "you selected the test Pyt Python Django113",
                Type = "postBack",
                Title = "Python Django",
            };
          
            cardButtons.Add(Button1);
            cardButtons.Add(Button2);
            cardButtons.Add(Button3);
            cardButtons.Add(Button4);
            cardButtons.Add(Button5);
           
            HeroCard jobCard = new HeroCard()
            {
                Buttons = cardButtons,
            };

            Attachment jobAttachment = jobCard.ToAttachment();
            reply.Attachments.Add(jobAttachment);
            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
        }

        private void android_test(Activity reply)
        {
            reply.Attachments = new List<Attachment>();
            List<CardAction> cardButtons = new List<CardAction>();
            CardAction Button1 = new CardAction()
            {
                Value = "you selected the test And Android Essential Training93",
                Type = "postBack",
                Title = "Android Studio Essential Training"
            };
            CardAction Button2 = new CardAction()
            {
                Value = "you selected the test And Communicating User94",
                Type = "postBack",
                Title = "Communicating with the User",
            };
            CardAction Button3 = new CardAction()
            {
                Value = "you selected the test And Local Data Storage95",
                Type = "postBack",
                Title = "Local Data Storage",
            };
            CardAction Button4 = new CardAction()
            {
                Value = "you selected the test And Animations and Transitions96",
                Type = "postBack",
                Title = "Animations and Transitions",
            };
            CardAction Button5 = new CardAction()
            {
                Value = "you selected the test And Unit Testing97",
                Type = "postBack",
                Title = "Unit Testing",
            };
            CardAction Button6 = new CardAction()
            {
                Value = "you selected the test And Distributing App98",
                Type = "postBack",
                Title = "Distributing App",
            };
            
            cardButtons.Add(Button1);
            cardButtons.Add(Button2);
            cardButtons.Add(Button3);
            cardButtons.Add(Button4);
            cardButtons.Add(Button5);
            cardButtons.Add(Button6);

            HeroCard jobCard = new HeroCard()
            {
                Buttons = cardButtons,
            };

            Attachment jobAttachment = jobCard.ToAttachment();
            reply.Attachments.Add(jobAttachment);
            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
        }

        private void ios_test(Activity reply)
        {
            reply.Attachments = new List<Attachment>();
            List<CardAction> cardButtons = new List<CardAction>();
            CardAction Button1 = new CardAction()
            {
                Value = "you selected the test Ios Swift Essential99",
                Type = "postBack",
                Title = "Swift 3 Essential"
            };
            CardAction Button2 = new CardAction()
            {
                Value = "you selected the test Ios Xcode Essentials100",
                Type = "postBack",
                Title = "Xcode Essentials",
            };
            CardAction Button3 = new CardAction()
            {
                Value = "you selected the test Ios Designing User Interface101",
                Type = "postBack",
                Title = "Designing User Interface",
            };
            CardAction Button4 = new CardAction()
            {
                Value = "you selected the test Ios Application Architecture102",
                Type = "postBack",
                Title = "Application Architecture",
            };
            CardAction Button5 = new CardAction()
            {
                Value = "you selected the test Ios Distributing Your App103",
                Type = "postBack",
                Title = "Distributing Your App",
            };  

            cardButtons.Add(Button1);
            cardButtons.Add(Button2);
            cardButtons.Add(Button3);
            cardButtons.Add(Button4);
            cardButtons.Add(Button5);

            HeroCard jobCard = new HeroCard()
            {
                Buttons = cardButtons,
            };

            Attachment jobAttachment = jobCard.ToAttachment();
            reply.Attachments.Add(jobAttachment);
            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
        }

        private void cross_test(Activity reply)
        {
            reply.Attachments = new List<Attachment>();
            List<CardAction> cardButtons = new List<CardAction>();
            CardAction Button1 = new CardAction()
            {
                Value = "you selected the test Pyt Programming Foundations",
                Type = "postBack",
                Title = "Programming Foundations"
            };
            CardAction Button2 = new CardAction()
            {
                Value = "you selected the test Pyt Essentials",
                Type = "postBack",
                Title = "Essentials",
            };
            CardAction Button3 = new CardAction()
            {
                Value = "you selected the test Pyt Python GUI",
                Type = "postBack",
                Title = "Python GUI",
            };
            CardAction Button4 = new CardAction()
            {
                Value = "you selected the test Pyt Code Clinic",
                Type = "postBack",
                Title = "Code Clinic",
            };
            CardAction Button5 = new CardAction()
            {
                Value = "you selected the test Pyt Python Django",
                Type = "postBack",
                Title = "Python Django",
            };

            cardButtons.Add(Button1);
            cardButtons.Add(Button2);
            cardButtons.Add(Button3);
            cardButtons.Add(Button4);
            cardButtons.Add(Button5);

            HeroCard jobCard = new HeroCard()
            {
                Buttons = cardButtons,
            };

            Attachment jobAttachment = jobCard.ToAttachment();
            reply.Attachments.Add(jobAttachment);
            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
        }

        private void test_type(Activity reply)
        {
            reply.Attachments = new List<Attachment>();
            List<CardAction> cardButtons = new List<CardAction>();
            CardAction Button1 = new CardAction()
            {
                Value = "you want to give basic level test",
                Type = "postBack",
                Title = "Basic"
            };
            CardAction Button2 = new CardAction()
            {
                Value = "you want to give medium level test",
                Type = "postBack",
                Title = "Medium",
            };
            CardAction Button3 = new CardAction()
            {
                Value = "you want to give expert level test",
                Type = "postBack",
                Title = "Expert",
            };
            cardButtons.Add(Button1);
            cardButtons.Add(Button2);
            cardButtons.Add(Button3);
            HeroCard jobCard = new HeroCard()
            {
                Buttons = cardButtons,
            };

            Attachment jobAttachment = jobCard.ToAttachment();
            reply.Attachments.Add(jobAttachment);
            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
        }

        private void reffered_to_choice(string occupy, Activity reply)
        {

            switch (occupy)
            {

                case "Design":
                    select_design(reply);
                    break;

                case "Development":
                    select_development(reply);
                    break;

                case "MarketingSales":
                    select_marketing(reply);
                    break;
            }


        }
        //private string refferedtochoice(string id, Activity reply)
        //{
        //    var result = MongoUser.exist_user(id);
        //    var occupy = JsonConvert.DeserializeObject<MongoData>(result.ToJson());
        //    string rep = null;
        //    switch (occupy.professional.occupation_type)
        //    {
              
        //        case "Design":
        //            rep = col.ElementAt(14);
        //            select_design(reply);
        //            break;

        //        case "Development":
        //            rep = col.ElementAt(48);
        //            select_development(reply);
        //            break;
        //        case "MarketingSales":
        //            rep = col.ElementAt(22);
        //            select_marketing(reply);
        //            break;
           
             

        //    }

        //    return rep;
        //}

        private void selectCompany(Activity reply)
        {
            reply.Attachments = new List<Attachment>();
            List<CardAction> cardButtons = new List<CardAction>();
            CardAction Button1 = new CardAction()
            {
                Value = "ok you are interested in Design/Development Studio",
                Type = "postBack",
                Title = "Design/Development Studio"
            };
            CardAction Button2 = new CardAction()
            {
                Value = "ok you are interested in Financial Technology",
                Type = "postBack",
                Title = "Financial Technology"
            };
            CardAction Button3 = new CardAction()
            {
                Value = "ok you are interested in Financial Technology",
                Type = "postBack",
                Title = "Game Studio"
            };
            CardAction Button4 = new CardAction()
            {
                Value = "ok you are interested in ECommerce",
                Type = "postBack",
                Title = "ECommerce"
            };
            CardAction Button5 = new CardAction()
            {
                Value = "ok you are interested in Sales",
                Type = "postBack",
                Title = "Sales"
            };
            CardAction Button6 = new CardAction()
            {
                Value = "ok you are interested in Female Focused Technology",
                Type = "postBack",
                Title = "Female Focused Technology"
            };
            cardButtons.Add(Button1);
            cardButtons.Add(Button2);
            cardButtons.Add(Button3);
            cardButtons.Add(Button4);
            cardButtons.Add(Button5);
            cardButtons.Add(Button6);
            HeroCard jobCard = new HeroCard()
            {
                Buttons = cardButtons
            };

            Attachment jobAttachment = jobCard.ToAttachment();
            reply.Attachments.Add(jobAttachment);
            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;

        }

        private void choose_invest(Activity reply)
        {
            reply.Attachments = new List<Attachment>();
            List<CardAction> cardButtons = new List<CardAction>();
            CardAction Button1 = new CardAction()
            {
                Value = "you selected the test Investor",  //technology
                Type = "postBack",
                Title = "Start"
            };
            cardButtons.Add(Button1);
            HeroCard jobCard = new HeroCard()
            {
                Buttons = cardButtons
            };

            Attachment jobAttachment = jobCard.ToAttachment();
            reply.Attachments.Add(jobAttachment);
            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;

        }

        private void choose_tech(Activity reply)
        {
            reply.Attachments = new List<Attachment>();
            List<CardAction> cardButtons = new List<CardAction>();
            CardAction Button1 = new CardAction()
            {
                Value = "you selected the test Android",  //technology
                Type = "postBack",
                Title = "Android"
            };
            CardAction Button2 = new CardAction()
            {
                Value = "you selected the test IOS",
                Type = "postBack",
                Title = "IOS"
            };
            CardAction Button3 = new CardAction()
            {
                Value = "you selected the test Cross Platform",
                Type = "postBack",
                Title = "Cross Platform"
            };
            CardAction Button4 = new CardAction()
            {
                Value = "you selected the test Python",
                Type = "postBack",
                Title = "Python"
            };
            CardAction Button5 = new CardAction()
            {
                Value = "you selected the test PHP",
                Type = "postBack",
                Title = "PHP"
            };
            CardAction Button6 = new CardAction()
            {
                Value = "you selected the test C#",
                Type = "postBack",
                Title = "C#"
            };
            cardButtons.Add(Button1);
            cardButtons.Add(Button2);
            cardButtons.Add(Button3);
            cardButtons.Add(Button4);
            cardButtons.Add(Button5);
            cardButtons.Add(Button6);
            HeroCard jobCard = new HeroCard()
            {
                Buttons = cardButtons
            };

            Attachment jobAttachment = jobCard.ToAttachment();
            reply.Attachments.Add(jobAttachment);
            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;

        }

        //private void JobOptions(Activity reply)
        //{
        //    reply.Attachments = new List<Attachment>();
        //    List<CardAction> cardButtons = new List<CardAction>();
        //    CardAction Button1 = new CardAction()
        //    {
        //        Value = "http://localhost/webview/",//"Design is an interesting career choice. Before we use our magic sauce to match you let's get your profile setup.",
        //        Type = "openUrl",
        //        Title = "Design",
                
        //    };

        //    CardAction Button2 = new CardAction()
        //    {
        //        Value = "Development is an interesting career choice. Before we use our magic sauce to match you let's get your profile setup.",
        //        Type = "postBack",
        //        Title = "Development"
        //    };

        //    CardAction Button3 = new CardAction()
        //    {
        //        Value = "MarketingSales is an interesting career choice. Before we use our magic sauce to match you let's get your profile setup.",
        //        Type = "postBack",
        //        Title = "Marketing"
                

        //    };

        //    cardButtons.Add(Button1);
        //    cardButtons.Add(Button2);
        //    cardButtons.Add(Button3);

        //    HeroCard jobCard = new HeroCard()
        //    {
        //        Buttons = cardButtons,
                

        //    };

        //    Attachment jobAttachment = jobCard.ToAttachment();
        //    reply.Attachments.Add(jobAttachment);
        //    reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;

        //}

        private void job_position(Activity reply)
        {
            reply.Attachments = new List<Attachment>();
            List<CardAction> cardButtons = new List<CardAction>();
            CardAction Button1 = new CardAction()
            {
                Value = "ok you like the position of fulltime",
                Type = "postBack",
                Title = "Full Time",

            };

            CardAction Button2 = new CardAction()
            {
                Value = "ok you like the position of parttime",
                Type = "postBack",
                Title = "Part Time"
            };

            CardAction Button3 = new CardAction()
            {
                Value = "ok you like the position of contract",
                Type = "postBack",
                Title = "Contract"
                 
            };

            cardButtons.Add(Button1);
            cardButtons.Add(Button2);
            cardButtons.Add(Button3);

            HeroCard jobCard = new HeroCard()
            {
                Buttons = cardButtons,

            };

            Attachment jobAttachment = jobCard.ToAttachment();
            reply.Attachments.Add(jobAttachment);
            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;

        }

        private void job_options(Activity reply)
        {

            reply.Attachments = new List<Attachment>();
            List<CardAction> cardButtons1 = new List<CardAction>();
            List<CardAction> cardButtons2 = new List<CardAction>();
            List<CardAction> cardButtons3 = new List<CardAction>();
            List<CardImage> cardImages1 = new List<CardImage>();
            List<CardImage> cardImages2 = new List<CardImage>();
            List<CardImage> cardImages3 = new List<CardImage>();

            CardImage image1 = new CardImage()
            {
                Url = "http://smbibotapp20170804124326.azurewebsites.net/Images/abstract-logo.png"
            };
            CardImage image2 = new CardImage()
            {
                Url = "http://smbibotapp20170804124326.azurewebsites.net/Images/brand-design.png"

            };
            CardImage image3 = new CardImage()
            {
                Url = "http://smbibotapp20170804124326.azurewebsites.net/Images/logos-mountain.png"
            };
            CardAction Button1 = new CardAction()
            {
                Value = "Design is an interesting career choice. Before we use our magic sauce to match you let's get your profile setup.",
                Type = "postBack",
                Title = "Design"
            };

            CardAction Button2 = new CardAction()
            {
                Value = "Development is an interesting career choice. Before we use our magic sauce to match you let's get your profile setup.",
                Type = "postBack",
                Title = "Development"
            };

            CardAction Button3 = new CardAction()
            {
                Value = "MarketingSales is an interesting career choice. Before we use our magic sauce to match you let's get your profile setup.",
                Type = "postBack",
                Title = "Marketing"

            };

            cardButtons1.Add(Button1);
            cardButtons2.Add(Button2);
            cardButtons3.Add(Button3);
            cardImages1.Add(image1);
            cardImages2.Add(image2);
            cardImages3.Add(image3);

            //ThumbnailCard thmbcard1 = new ThumbnailCard()
            //{
            //    Buttons = cardButtons1,
            //    Images = cardImages1
            //};
            //ThumbnailCard thmbcard2 = new ThumbnailCard()
            //{
            //    Buttons = cardButtons2,
            //    Images = cardImages2
            //};
            //ThumbnailCard thmbcard3 = new ThumbnailCard()
            //{
            //    Buttons = cardButtons3,
            //    Images = cardImages3
            //};

            //Attachment jobAttachment1 = thmbcard1.ToAttachment();

            //Attachment jobAttachment2 = thmbcard2.ToAttachment();
            //Attachment jobAttachment3 = thmbcard3.ToAttachment();
            //reply.Attachments.Add(jobAttachment1);
            //reply.Attachments.Add(jobAttachment2);
            //reply.Attachments.Add(jobAttachment3);

            HeroCard jobCard1 = new HeroCard()
            {
                Buttons = cardButtons1,
                Images = cardImages1,

            };
            HeroCard jobCard2 = new HeroCard()
            {
                Buttons = cardButtons2,
                Images = cardImages2
            };

            HeroCard jobCard3 = new HeroCard()
            {
                Buttons = cardButtons3,
                Images = cardImages3
            };

            Attachment jobAttachment1 = jobCard1.ToAttachment();

            Attachment jobAttachment2 = jobCard2.ToAttachment();
            Attachment jobAttachment3 = jobCard3.ToAttachment();
            reply.Attachments.Add(jobAttachment1);
            reply.Attachments.Add(jobAttachment2);
            reply.Attachments.Add(jobAttachment3);
            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;


            //reply.Attachments = new List<Attachment>();
            //List<CardAction> cardButtons1 = new List<CardAction>();
            //List<CardAction> cardButtons2 = new List<CardAction>();
            //List<CardAction> cardButtons3 = new List<CardAction>();
            //List<CardImage> cardImages1 = new List<CardImage>();
            //List<CardImage> cardImages2 = new List<CardImage>();
            //List<CardImage> cardImages3 = new List<CardImage>();

            //CardImage image1 = new CardImage()
            //{

            //    Url = $"http://smbibotapp20170804124326.azurewebsites.net/Images/abstract-logo.jpg"

            //};
            //CardImage image2 = new CardImage()
            //{

            //    Url = $"http://smbibotapp20170804124326.azurewebsites.net/Images/brand-design.png"
            //};
            //CardImage image3 = new CardImage()
            //{
            //    Url = $"http://smbibotapp20170804124326.azurewebsites.net/Images/logos-mountain.png"
            //};
            //CardAction Button1 = new CardAction()
            //{
            //    Value = "Design is an interesting career choice. Before we use our magic sauce to match you let's get your profile setup.",
            //    Type = "postBack",
            //    Title = "Design",
            //};

            //CardAction Button2 = new CardAction()
            //{
            //    Value = "Development is an interesting career choice. Before we use our magic sauce to match you let's get your profile setup.",
            //    Type = "postBack",
            //    Title = "Development"
            //};

            //CardAction Button3 = new CardAction()
            //{
            //    Value = "MarketingSales is an interesting career choice. Before we use our magic sauce to match you let's get your profile setup.",
            //    Type = "postBack",
            //    Title = "Marketing"

            //};

            //cardButtons1.Add(Button1);
            //cardButtons2.Add(Button2);
            //cardButtons3.Add(Button3);
            //cardImages1.Add(image1);
            //cardImages2.Add(image2);
            //cardImages3.Add(image3);

            //HeroCard jobCard1 = new HeroCard()
            //{
            //    Buttons = cardButtons1,
            //    Images = cardImages1,

            //};
            //HeroCard jobCard2 = new HeroCard()
            //{
            //    Buttons = cardButtons2,
            //    Images = cardImages2
            //};

            //HeroCard jobCard3 = new HeroCard()
            //{
            //    Buttons = cardButtons3,
            //    Images = cardImages3
            //};


            //Attachment jobAttachment1 = jobCard1.ToAttachment();
            //Attachment jobAttachment2 = jobCard2.ToAttachment();
            //Attachment jobAttachment3 = jobCard3.ToAttachment();
            //reply.Attachments.Add(jobAttachment1);
            //reply.Attachments.Add(jobAttachment2);
            //reply.Attachments.Add(jobAttachment3);
            //reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;

        }

        private void personality_view(Activity reply)
        {
            var attachment = new
            {
                type = "template",
                payload = new
                {
                    template_type = "button",
                    text = data[1].Split().First() +" now that I have a grasp of what your looking for lets play a word game to unlock your personality and get insights into companies that would be right for you",
                    buttons = new[]
                    {
                      new
                      {
                        type = "web_url",
                        url = "https://smbibotapp20170804124326.azurewebsites.net/Webviews/index.html",
                        title = "Personality Test",
                        webview_height_ratio = "tall",
                        messenger_extensions = true
                      }
                    }
                }
            };

            reply.ChannelData = JObject.FromObject(new
            {
                attachment

            });
        }

        private void proj_prof(Activity reply)
        {
            reply.Attachments = new List<Attachment>();
            List<CardAction> cardButtons = new List<CardAction>();
            CardAction Button1 = new CardAction()
            {
                Value = "lets talk about your project history",
                Type = "postBack",
                Title = "project",

            };
            CardAction Button2 = new CardAction()
            {
                Value = "lets talk about your professional history",
                Type = "postBack",
                Title = "professional"
            };
            cardButtons.Add(Button1);
            cardButtons.Add(Button2);

            SuggestedActions actions = new SuggestedActions()
            {
                Actions = cardButtons,           
            };
                        
            reply.SuggestedActions = actions;
            ////reply.Attachments.Add(jobAttachment);
            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;

        }

        private void continueornot(Activity reply)
        {
            reply.Attachments = new List<Attachment>();
            List<CardAction> cardButtons = new List<CardAction>();
            CardAction Button1 = new CardAction()
            {
                Value = "you are matched with the positions best suited for you",
                Type = "postBack",
                Title = "Yes",
            };
            CardAction Button2 = new CardAction()
            {
                Value = "lets talk about your professional history",
                Type = "postBack",
                Title = "No"
            };
            cardButtons.Add(Button1);
            cardButtons.Add(Button2);
          
            SuggestedActions actions = new SuggestedActions()
            {
                Actions = cardButtons,
            
            };

            reply.SuggestedActions = actions;
            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;

        }
        private void test(Activity reply, string opt, int mod)
        {
           
            reply.Attachments = new List<Attachment>();
            List<CardAction> cardButtons = new List<CardAction>();
            string[] option = opt.Split('@');
            CardAction Button1 = new CardAction()
            {
                Value = "A your are still in test mode" + mod,
                Type = "postBack",
                Title = option[0],
            };
            CardAction Button2 = new CardAction()
            {
                Value = "B your are still in test mode" + mod,
                Type = "postBack",
                Title = option[1],
            };
            CardAction Button3 = new CardAction()
            {
                Value = "C your are still in test mode" + mod,
                Type = "postBack",
                Title = option[2],
            };
            CardAction Button4 = new CardAction()
            {
                Value = "D your are still in test mode" + mod,
                Type = "postBack",
                Title = option[3],
            };
            cardButtons.Add(Button1);
            cardButtons.Add(Button2);
            cardButtons.Add(Button3);
            cardButtons.Add(Button4);
            HeroCard jobCard = new HeroCard()
            {
                Buttons = cardButtons,
            };

            Attachment jobAttachment = jobCard.ToAttachment();
            reply.Attachments.Add(jobAttachment);
            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
        }

        private Tuple<bool, string> input_validation(string user_input, int count)
        {
            var t = new Tuple<bool, string>(true, "");
            switch (count)
            {

                case 1:
                case 2:
                case 3:
                case 9:
                    t = IsValidAlphabet(user_input, count);
                    if (t.Item1)
                    {
                        return new Tuple<bool, string>(true, t.Item2);
                    }
                    else
                    {
                        return new Tuple<bool, string>(false, t.Item2); ;
                    }
                    break;

                case 4:
                    t = IsValidEmail(user_input);
                    if (t.Item1)
                    {
                        return new Tuple<bool, string>(true, t.Item2);
                    }
                    else
                    {
                        return new Tuple<bool, string>(false, t.Item2);
                    }
                    break;
                case 5:
                case 6:
                case 7:
                case 8:
                case 10:
                    t = IsValidNumber(user_input, count);
                    if (t.Item1)
                    {
                        return new Tuple<bool, string>(true, t.Item2);
                    }
                    else
                    {
                        return new Tuple<bool, string>(false, t.Item2);
                    }
                
                default:
                    return new Tuple<bool, string>(true, "");
                    break;
            }

            //if (count == 1 || count == 2 || count == 4 || count == 5 || count == 7 || count == 9)
            //{
            //    if (Regex.IsMatch(user_input, @"^[a-zA-Z]+$"))
            //    {
            //        return true;
            //    }
            //    else
            //    {
            //        return false;
            //    }
            //}

            //else if (count == 3 || count == 8 || count == 10)
            //{
            //    if (Regex.IsMatch(user_input, @"^[0-9]+$"))
            //    {
            //        return true;
            //    }
            //    else
            //    {
            //        return false;
            //    }
            //}
            //else if (count == 11 || count == 12)
            //{
            //    if (Regex.IsMatch(user_input,
            //        @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
            //        @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$"))
            //    {
            //        return true;
            //    }
            //    else
            //    {
            //        return false;
            //    }
            //}
            //else
            //{
            //    return true;
            //}


        }

        bool invalid = false;

        public Tuple<bool, string> IsValidEmail(string strIn)
        {
            invalid = false;
            if (String.IsNullOrEmpty(strIn))
                return new Tuple<bool, string>(false, "");

            // Use IdnMapping class to convert Unicode domain names.
            try
            {
                strIn = Regex.Replace(strIn, @"(@)(.+)$", this.DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));
            }
            catch (RegexMatchTimeoutException)
            {
                return new Tuple<bool, string>(false, "");
            }

            if (invalid)
                return new Tuple<bool, string>(false, "");

            // Return true if strIn is in valid e-mail format.
            try
            {
                bool valid = Regex.IsMatch(strIn,
                      @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                      @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                      RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
                return new Tuple<bool, string>(valid, "");
            }
            catch (RegexMatchTimeoutException)
            {
                return new Tuple<bool, string>(false, "");
            }
        }
        public Tuple<bool, string> IsValidAlphabet(string strIn, int count)
        {

            if (Regex.IsMatch(strIn, @"^[a-zA-Z\s]+$"))
            {
                switch (count)
                {

                    case 4:
                        string str = strIn.ToLower();
                        if (str.Contains("male") || str.Contains("female") || str.Contains("guy") || str.Contains("girl") || str.Contains("disclose"))
                        {
                            return new Tuple<bool, string>(true, "");
                        }
                        else
                        {
                            return new Tuple<bool, string>(false, "Gender Must be Male/Female/Donot Want to Disclose");
                        }
                        break;
                    default:
                        return new Tuple<bool, string>(true, "");
                        break;
                }
            }
            else
            {
                return new Tuple<bool, string>(false, "");
            }
        }
        public Tuple<bool, string> IsValidNumber(string strIn, int count)
        {
            try
            {

                switch (count)
                {
                    case 3:
                        if (Regex.IsMatch(strIn, @"^[0-9]{2}"))
                        {
                            int strr = Int32.Parse(strIn);
                            if (strr > 15 && strr < 55)
                            {
                                return new Tuple<bool, string>(true, "");
                            }
                            else
                            {
                                return new Tuple<bool, string>(false, "Age must be a number between 15-55");
                            }
                        }
                        else
                        {
                            return new Tuple<bool, string>(false, "Age must be a number between 15-55");
                        }
                        break;
                    case 8:
                        if (Regex.IsMatch(strIn, @"^[0-9]{4}"))
                        {
                            int strr = Int32.Parse(strIn);
                            if (strr >= 1980 && strr <= DateTime.Now.Year)
                                return new Tuple<bool, string>(true, "");
                            else
                                return new Tuple<bool, string>(false, "Year must be between 1980-Present_Year");

                        }
                        else
                        {
                            return new Tuple<bool, string>(false, "Year must be between 1980-Present_Year");
                        }
                        break;
                    case 10:
                        if ((Regex.IsMatch(strIn, @"^[0-9]")))
                        {
                            int strr = Int32.Parse(strIn);
                            if (strr > 0)
                                return new Tuple<bool, string>(true, "");
                            else
                                return new Tuple<bool, string>(false, "No. of Projects must be greater than zero");
                        }
                        else
                        {
                            return new Tuple<bool, string>(false, "No. of Projects must be greater than zero");
                        }
                        break;
                    default:
                        return new Tuple<bool, string>(true, "");
                }
            }

            catch
            { return new Tuple<bool, string>(false, ""); }


        }

        private string DomainMapper(Match match)
        {
            // IdnMapping class with default property values.
            IdnMapping idn = new IdnMapping();

            string domainName = match.Groups[2].Value;
            try
            {
                domainName = idn.GetAscii(domainName);
            }
            catch (ArgumentException)
            {
                invalid = true;
            }
            return match.Groups[1].Value + domainName;
        }
    }

}