var mongoose = require('mongoose');
var Schema = mongoose.Schema;

var postSchema = new mongoose.Schema({
	created_by: String,		//should be changed to ObjectId, ref "User"
	created_at: {type: Date, default: Date.now},
	text: String
});

var userSchema = new mongoose.Schema({
	username: String,
	password: String, //hash created from password
	created_at: {type: Date, default: Date.now}
});

//////////////////////////////////  version 1.1   ////////////////////////////////////
var jobSchema = new mongoose.Schema({
	position: String,
	valid_date: String,
    location: String,
    salary_offered: String,
    contract_type: String,
    experience_type: String,
    role_description: String,
	modules: []
});
//////////////////////////////////  version 1.1   ////////////////////////////////////

mongoose.model('Post', postSchema);
mongoose.model('User', userSchema);
mongoose.model('Job' , jobSchema); // version 1.1

