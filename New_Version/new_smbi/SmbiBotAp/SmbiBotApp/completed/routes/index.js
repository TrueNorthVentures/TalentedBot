var express = require('express');
var router = express.Router();
//var mongoose = require('mongoose'); // version 1.1
//var Job = mongoose.model('Job');    // version 1.1
var Job = require('../models/models');
var MongoClient = require('mongodb').MongoClient;
var url="mongodb://rizwan:rizwan@ds127883.mlab.com:27883/talenthome";
//const MongoClient = require('mongodb').MongoClient;
var db;
MongoClient.connect('mongodb://rizwan:rizwan@ds127883.mlab.com:27883/talenthome', (err, database) => {
  if (err) return console.log(err)
  db = database;
});

/* GET home page. */

router.get('/', function(req, res, next) {
	    res.render('bot', { title: "Chirp"});
});

//////////////////////////////////////  version 1.0  ////////////////////////////////////////

router.get('/retrieve', function(req, res, next) {
     var quer = {"basic.first_name":req.query.firstname};
     db.collection("botusers").find(quer).toArray(function(err,result){
     if (err) return console.log(err);
     return res.json(result);
     db.close();
    });
});


router.get('/tracks', function(req, res, next) {
    
     var url = "mongodb://rizwan:rizwan@ds121495.mlab.com:21495/bottest";
     MongoClient.connect(url,(err, db) => {
     if (err) return console.log(err)
     db.collection("nested").find().toArray(function(err,result){
     if (err) return console.log(err);
     return res.json(result);
     db.close();
    })     
 })
});


router.post('/tracks', function(req, res, next) {
    
    var job = new Job({
        
    position: req.body.position,
	valid_date: req.body.valid_date,
    location: req.body.location,
    salary_offered: req.body.salary_offered,
    contract_type: req.body.contract_type,
    experience_type: req.body.experience_type,
    role_description: req.body.role_description,
                
        
    });
    job.save();
      
});

module.exports = router;

//////////////////////////////////////  version 1.0  ////////////////////////////////////////

//router.get('/retrieve', function(req, res, next) {
//	//res.render('index', { title: "Chirp"});
//   // MongoClient.connect(url, function(err, db) {
// //    if (err) return console.log(err);
//    // var quer = req.params["basic.first_name"];
//    // var fir = req.params.first;
//    // var las = req.params.last;
//      //var quer = req.query.first;
//    // var que = req.firstname;
//    //.body["basic.first_name"];
//     var quer = {"basic.first_name":req.query.firstname};
//     db.collection("botusers").find(quer).toArray(function(err,result){
//     if (err) return console.log(err);
//     return res.json(result);
//         // res.send(result);
//    // return res.json(result);
//         // console.log('data retrieved');
//     //res.json(result);  
//   //  res.status(status).send(body);
//    // res.render('bot.ejs');
//     db.close();
//    });
//   });
//// });

//////////////////////////////////////  version 1.0  ////////////////////////////////////////s