const express = require('express');
const morgan = require('morgan');
const exphbs = require('express-handlebars');
const path = require('path');
const bodyPArser=require('body-parser');
const app = express();

// settings

app.set('port', process.env.PORT || 4000);
app.set('views', path.join(__dirname, 'views'));
app.engine('.hbs', exphbs({
    defaultLayout: 'main',
    extname: '.hbs'    
}));
app.set('view engine', '.hbs');

// middlewares

app.use(morgan('dev'));
app.use(express.urlencoded({extended: false}));
//app.use(bodyPArser.urlencoded({extended: false}));
app.use(bodyPArser.json());
// routes

app.use(require('./routes/index'));

// static files

app.use(express.static(path.join(__dirname, 'public')));

module.exports = app;