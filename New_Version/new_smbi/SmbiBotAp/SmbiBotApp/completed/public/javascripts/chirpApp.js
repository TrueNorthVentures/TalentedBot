var app = angular.module('chirpApp', ['ngRoute', 'ngResource']).run(function ($rootScope) {
    $rootScope.authenticated = false;
    $rootScope.current_user = '';

    $rootScope.signout = function () {
        $http.get('auth/signout');
        $rootScope.authenticated = false;
        $rootScope.current_user = '';
    };
});

app.config(function ($routeProvider) {
    $routeProvider
        //the timeline display
        .when('/', {
            templateUrl: 'main.html',
            controller: 'mainController'
        })
        //the login display
        .when('/login', {
            templateUrl: 'login.html',
            controller: 'authController'
        })
        // bot users data
        .when('/bot', {
            templateUrl: 'botuser.html',
            controller: 'botController'
        })
        .when('/create', {
            templateUrl: 'createjob.html',
            controller: 'jobController'
        })
        .when('/mcqs', {
            templateUrl: 'mcq.html',
            controller: 'mcqController'
        })

        //the signup display
        .when('/signup', {
            templateUrl: 'register.html',
            controller: 'authController'
        });
});

app.factory('postService', function ($resource) {
    return $resource('/api/posts/:id');
});

app.factory('botService', function ($resource) {
    return $resource('/index/retrieve');
});

app.factory('jobService', function ($resource) {
    return $resource('/index/tracks');
});

app.factory('mcqService', function ($resource) {
    return $resource('/index/tracks');
});



app.controller('mainController', function (postService, $scope, $rootScope) {
    $scope.posts = postService.query();
    $scope.newPost = { created_by: '', text: '', created_at: '' };

    $scope.post = function () {
        $scope.newPost.created_by = $rootScope.current_user;
        $scope.newPost.created_at = Date.now();
        postService.save($scope.newPost, function () {
            $scope.posts = postService.query();
            $scope.newPost = { created_by: '', text: '', created_at: '' };
        });
    };
});



app.controller('mcqController', function (postService, $scope, $rootScope) {

    $scope.mutipleChoices = {
        1: '',
        2: '',
        3: '',
        4: '',
        5: '',
        6: '',
        7: '',

    };

    var personalityFactor = {
        Extraverted: 0,
        Introverted: 0,
        Sensing: 0,
        Intuitive: 0,
        Thinking: 0,
        Feeling: 0,
        Judging: 0,
        Perceiving: 0
    };

    $scope.disp = [];
    $scope.mcqdata = function () {
        var number = 4;
        var rem = 1;
        for (var i = 1; i <= 7; i++) {
            rem = i;
            if (i > number) {
                rem = i - number;
                if (rem == 4) {
                    number = number + 4;
                }

            }
            switch (rem) {
                case 1:

                    if ($scope.mutipleChoices[i] > 0) {
                        personalityFactor.Extraverted = personalityFactor.Extraverted + parseInt($scope.mutipleChoices[i]);

                        personalityFactor.Introverted = personalityFactor.Introverted + (100 - parseInt($scope.mutipleChoices[i]));
                    }
                    else if ($scope.mutipleChoices[i] < 0) {
                        personalityFactor.Introverted = personalityFactor.Introverted - parseInt($scope.mutipleChoices[i]);

                        personalityFactor.Extraverted = personalityFactor.Extraverted + (100 + parseInt($scope.mutipleChoices[i]));

                    }

                    break;

                case 2:

                    if ($scope.mutipleChoices[i] > 0) {
                        personalityFactor.Sensing = personalityFactor.Sensing + parseInt($scope.mutipleChoices[i]);

                        personalityFactor.Intuitive = personalityFactor.Intuitive + (100 - parseInt($scope.mutipleChoices[i]));
                    }
                    else if ($scope.mutipleChoices[i] < 0) {
                        personalityFactor.Intuitive = personalityFactor.Intuitive - parseInt($scope.mutipleChoices[i]);

                        personalityFactor.Sensing = personalityFactor.Sensing + (100 + parseInt($scope.mutipleChoices[i]));
                    }

                    break;

                case 3:

                    if ($scope.mutipleChoices[i] > 0) {
                        personalityFactor.Thinking = personalityFactor.Thinking + parseInt($scope.mutipleChoices[i]);

                        personalityFactor.Feeling = personalityFactor.Feeling + (100 - parseInt($scope.mutipleChoices[i]));
                    }
                    else if ($scope.mutipleChoices[i] < 0) {
                        personalityFactor.Feeling = personalityFactor.Feeling - parseInt($scope.mutipleChoices[i]);

                        personalityFactor.Thinking = personalityFactor.Sensing + (100 + parseInt($scope.mutipleChoices[i]));
                    }

                    break;

                case 4:

                    if ($scope.mutipleChoices[i] > 0) {
                        personalityFactor.Judging = personalityFactor.Judging + parseInt($scope.mutipleChoices[i]);

                        personalityFactor.Perceiving = personalityFactor.Perceiving + (100 - parseInt($scope.mutipleChoices[i]));
                    }
                    else if ($scope.mutipleChoices[i] < 0) {
                        personalityFactor.Perceiving = personalityFactor.Perceiving - parseInt($scope.mutipleChoices[i]);

                        personalityFactor.Judging = personalityFactor.Judging + (100 + parseInt($scope.mutipleChoices[i]));

                    }

                    break;


            }
            //$scope.disp.push($scope.multipleChoices.1); 
        }
        var j = 0;
        var person = '';
        for (var i = 1; i <= 4; i++) {
            var show = Object.keys(personalityFactor)[0];
            //show  = Object.keys(personalityFactor)[1];
            //show  = Object.keys(personalityFactor)[2];
            //show  = Object.keys(personalityFactor)[3];
            // show  =  Object.values(personalityFactor)[0];


            //var sh =  show.charAt(0);
            //var person = '';  
            if (Object.values(personalityFactor)[j] / 5 > Object.values(personalityFactor)[j + 1] / 5) {
                person = person + Object.keys(personalityFactor)[j].charAt(0);
            }
            else {
                person = person + Object.keys(personalityFactor)[j + 1].charAt(0);

            }

            j = j + 2;
        }
        $scope.display = person;
    };


    $scope.returntobot = function () {
        (function (d, s, id) {
            var js, fjs = d.getElementsByTagName(s)[0];
            if (d.getElementById(id)) { return; }
            js = d.createElement(s); js.id = id;
            js.src = "//connect.facebook.com/en_US/messenger.Extensions.js";
            fjs.parentNode.insertBefore(js, fjs);
        }(document, 'script', 'Messenger'));


        window.extAsyncInit = function () {
            var text = "welcome back to the bot";
            document.getElementById("success").onclick = function () {
                MessengerExtensions.requestCloseBrowser(function success() {

                    window.location.href = "https://smbibotapp20170804124326.azurewebsites.net/api/Callback?display=" + encodeURI(text);

                }, function error(err) {
                });

            }
        };


    }
});





//////////////////////////////////  version 1.0   ////////////////////////////////////

app.controller('botController', function (botService, $scope) {
    $scope.userName = {
        firstName: '',
        lastName: ''
    };
    $scope.dis = [];
    $scope.botdata = function () {
        $scope.dis = botService.query({ firstname: $scope.userName.firstName });
    }
});

//////////////////////////////////  version 1.0   ////////////////////////////////////

app.controller('jobController', function (jobService, $scope) {

    $scope.tracks = [];
    $scope.weight = '';
    $scope.modules = [];
    $scope.selected = '';
    $scope.counter = '';
    $scope.jobSchema = {                  //   version 1.1
        position: '',
        valid_date: '',
        location: '',
        salary_offered: '',
        contract_type: '',
        experience_type: '',
        role_description: '',
        Modules: []
    };
    $scope.tracks = jobService.query();
    // users = $scope.tracks;


    $scope.displaymodules = function () {        //   version 1.1 
        $scope.selected = $scope.jobSchema.position;


    }
    //     $scope.getcounter = function(weight){        //   version 1.2
    //          
    //         $scope.modules.push(weight);
    //         //for(var i = 0 ; i < index ; i++)
    //        // {
    //             //if($scope.jobSchema.Modules[i].module_name == '') 
    //             //{
    //            //    $scope.jobSchema.Modules[i].module_name = modulename;
    //        //     }
    //             //  $scope.counter = index;
    //          // $scope.jobSchema.Modules.push({module_name:modulename,weightage:''});
    //         //}
    //     }
    $scope.createjob = function () {        //   version 1.2

        angular.forEach($scope.tracks, function (value, key) {
            angular.forEach(value.TestStructure.Courses, function (value, key) {
                angular.forEach(value.Tracks, function (value, key) {
                    if (value.track_name == $scope.selected) {
                        angular.forEach(value.Modules, function (value, key) {

                            // $scope.modules.push(value.module_name); 
                            $scope.jobSchema.Modules.push({ module_name: value.module_name, weightage: '' });

                        });

                    }
                });
            });
        });

        for (var i = 0; i < 4; i++) {
            $scope.jobSchema.Modules[i].weightage = $scope.modules[0];

            //            $scope.jobSchema.Modules[i].weightage = document.getElementsByName($scope.jobSchema.Modules[i].module_name).value; 
            //            
            //             $scope.jobSchema.Modules[i].weightage = $scope.jobSchema.Modules[i].module_name; 
            //            $scope.jobSchema.Modules[i].weightage = document.getElementsByName($scope.jobSchema.Modules[i].module_name).value; 
            //            
        }

        jobService.save($scope.jobSchema, function () {
            jobService.query();
        });


    }

    //$scope.modules = $scope.jobSchema.trackName;
    //    angular.forEach($scope.tracks.TestStructure.Courses, function(value, key) {
    //    angular.forEach(value.Tracks, function(value, key) {
    //    $scope.modules.push(value.track_name);
    //      });
    //    });
    //$scope.tracks.TestStructure.Courses.forEach(function(cou){ 
    //    cou.Courses.forEach(function(tra) { 
    //    tra.Tracks.forEach(function(t){
    //    $scope.display.push(t.track_name);
    //    })
    //    });
});

//    $scope.tracks.TestStructure.Courses.forEach(function(cou){ 
//    cou.Tracks.forEach(function(tra){ 
//        $scope.display.push(tra.track_name);
//    })
//        
//});
//$scope.view = $scope.display;
//$scope.rettracks = function() {
//        for(var i = 0 ; i <= 2; i++)
//        {
//          for(var j = 0 ; j < $scope.tracks.TestStructure.Courses[i].Tracks.length; j++)
//          {
//       //     display.push($scope.tracks.TestStructure.Courses[i].Tracks[j].track_name);
//        //    $scope.tracksName.push($scope.tracks.TestStructure.Courses[i].Tracks[j]);
//            $scope.tracksName.push($scope.tracks.TestStructure.Courses[i].Tracks[j].track_name);
//          }
//        }
//    return $scope.tracksName;
//        $scope.trackName = display;       


//});


//app.factory('botService', function($resource){
//    return $resource('/index/retrieve',
//                        
//                    //  { q: 'Syed' }, // Query parameters
//                    //  {'query': { method: 'GET',isArray:true}}
//                    );
//});

//app.controller('botController', function(botService,$scope){
//	//$scope.posts = postService.query();
//	//var user = {
//    //    firstName: '', 
//     //   lastName: ''
//   // };
//   // $scope.result = user;
//    $scope.userName = {
//        firstName: '', 
//        lastName: ''
//    };
//  //  var pla = [];  
//    $scope.dis = [];
//	//$scope.users_info = botService.query();
//    $scope.botdata = function() {
//   // var data = {"basic.first_name":$scope.userName.firstName,
//         //       "basic.last_name":$scope.userName.lastName};
//        $scope.dis = botService.query({firstname:$scope.userName.firstName});
//     //   $scope.dis = pla;
//     //   var data = {"first_name":$scope.userName.firstName};
//     
//
////         botService.query({firstname:$scope.userName.firstName}).$promise.then(function(results) {
////           // console.log(results);
////           $scope.dis = results;
////        }, function(error) {
////          // console.log(error);
////           $scope.dis = [];
////        });
//  botService.query({firstname:data.first_name});
////        $http.get('/retrieve', data)
////                .success(function (data, status, headers, config) {
////                    $scope.PostDataResponse = data;
////                })
////                .error(function(data, status, header, config){
////                    $scope.PostDataResponse = "Data: " + status;
////
////                });
//    }
//   
//});


//  
//app.controller('createJob', function($scope,$rootScope,$http){
//	$scope.display = {};
//    $scope.userName = {
//        firstName: '', 
//        lastName: ''
//    };
//	
//    $http({
//            url: 'http://localhost:8080/index/retrieve',
//            method: 'GET',
//            params: {"basic.first_name":$scope.userName.firstName,
//                     "basic.last_name":$scope.userName.lastName} //at server it will be req.query.email
//        }).success(function(res) {
//
//            $scope.display = res;
//             //access returned res here
//
//        }, function(error) {
//            //handle error here
//        });
//    
//      // $scope.create = function() {
//       //$http.get("mongodb://rizwan:rizwan@ds127883.mlab.com:27883/talenthome?first_name=" + $scope.userName.firstName + 
//       //"&last_name=" + $scope.userName.lastName).
//          //      success(function (data) {
//         //               $scope.display = data;   
//          //      })
//
//       //  }
//       
//});

//   $scope.tracks = {
//        TestStructure:{
//        Courses :[
//        {
//            course_name : 'Design', 
//            Tracks:[
//                {
//                    track_name : 'UX/UI'
//                },
//                {
//                    track_name : 'Graphic'
//                }
//            ]
//        },
//        {
//            course_name : 'Development',
//            Tracks:[
//                {
//                    track_name : 'Application'
//                },
//                {
//                    track_name : 'Mobile'
//                }
//                            
//            ]
//        },
//        {
//            course_name : 'Marketing',
//            Tracks:[
//                {
//                    track_name : 'Digital'
//                },
//                {
//                    track_name : 'Sales'
//                }
//                            
//                
//                
//            ]
//        }
//        ]      
//        }
//    };
//           


app.controller('authController', function ($scope, $http, $rootScope, $location) {
    $scope.user = { username: '', password: '' };
    $scope.error_message = '';

    $scope.login = function () {
        $http.post('/auth/login', $scope.user).success(function (data) {
            if (data.state == 'success') {
                $rootScope.authenticated = true;
                $rootScope.current_user = data.user.username;
                $location.path('/');
            }
            else {
                $scope.error_message = data.message;
            }
        });
    };

    $scope.register = function () {
        $http.post('/auth/signup', $scope.user).success(function (data) {
            if (data.state == 'success') {
                $rootScope.authenticated = true;
                $rootScope.current_user = data.user.username;
                $location.path('/');
            }
            else {
                $scope.error_message = data.message;
            }
        });
    };
});