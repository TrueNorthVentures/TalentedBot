var express = require('express');
var path = require('path');
var favicon = require('serve-favicon');
var logger = require('morgan');
var cookieParser = require('cookie-parser');
var bodyParser = require('body-parser');
var session = require('express-session');
var passport = require('passport');
var index = require('./routes/index');
//initialize mongoose schemas
require('./models/models');
var index = require('./routes/index');
var api = require('./routes/api');
var authenticate = require('./routes/authenticate')(passport);
var mongoose = require('mongoose');                         //add for Mongo support
mongoose.connect('mongodb://rizwan:rizwan@ds127883.mlab.com:27883/talenthome');
//const MongoClient = require('mongodb').MongoClient;
//var db;
//MongoClient.connect('mongodb://rizwan:rizwan@ds127883.mlab.com:27883/talenthome', (err, database) => {
//  if (err) return console.log(err)
//  db = database;
//});

var app = express();

// view engine setup
app.set('views', path.join(__dirname, 'views'));
app.set('view engine', 'ejs');
//app.get('/crejobs',index);
// uncomment after placing your favicon in /public
//app.use(favicon(__dirname + '/public/favicon.ico'));
app.use(logger('dev'));
app.use(session({
  secret: 'keyboard cat'
}));
app.use(bodyParser.json());
app.use(bodyParser.urlencoded({ extended: false }));
app.use(cookieParser());
app.use(express.static(path.join(__dirname, 'public')));
app.use(passport.initialize());
app.use(passport.session());

//app.use('/', index);
app.use('/auth', authenticate);
app.use('/api', api);
app.use('/index',index);

//app.get('/', (req, res)=> {
//	//res.render('index', { title: "Chirp"});
////     MongoClient.connect(url, function(err, db) {
////    if (err) throw err;
//   // var query = {first_name:$scope.userName.firstName};
//    db.collection("botusers").find().toArray(function(err, result) {
//    if (err) throw err;
//    res.render('bot.ejs', {display:result});
//    db.close();
//    });
//   });
// catch 404 and forward to error handler
app.use(function(req, res, next) {
    var err = new Error('Not Found');
    err.status = 404;
    next(err);
});

//// Initialize Passport
var initPassport = require('./passport-init');
initPassport(passport);

// error handlers

// development error handler
// will print stacktrace
if (app.get('env') === 'development') {
    app.use(function(err, req, res, next) {
        res.status(err.status || 500);
        res.render('error', {
            message: err.message,
            error: err
        });
    });
}

// production error handler
// no stacktraces leaked to user
app.use(function(err, req, res, next) {
    res.status(err.status || 500);
    res.render('error', {
        message: err.message,
        error: {}
    });
});


module.exports = app;