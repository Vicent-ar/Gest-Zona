const admin = require('firebase-admin');
let reference;
var serviceAccount = require("../../covid-register.json");
const bucketName = 'covid-register-3e647.appspot.com';
const {Storage} = require('@google-cloud/storage');
const uuid = require('uuid-v4');
const uuid2=require('uuid-v4');
const firebase=require('firebase');
const Handelbars=require('express-handlebars');

admin.initializeApp({
    credential: admin.credential.cert(serviceAccount),
    databaseURL: 'https://covid-register-3e647-default-rtdb.europe-west1.firebasedatabase.app/',
    storageBucket: bucketName  
});
const { Router } = require('express');
const { app } = require('firebase-admin');
const { parse } = require('uuid');
const router = Router();
var bucket=admin.storage().bucket();

const db=admin.database();


router.get('/', (req, res)=>{
  var eventoName=[];
  var eventoID=[];
  
    db.ref('Eventos').once('value', (snapshot)=>{
        var data = snapshot.val();
        var keys=Object.keys(data);
        var fechaActual=new Date;
 
        for(var i=0;i<keys.length;i++){          
           var EventoActivo=data[keys[i]].activoEvento;
           if(EventoActivo=="Si"){
             var fechaIni=(data[keys[i]].fechaEvento);
             var fechaIniSplit=fechaIni.split('/');
             var diaIniSplit=fechaIniSplit[0];
             var mesIniSplit=fechaIniSplit[1];
             var anyoIniSplit=fechaIniSplit[2];
             var horaIni=(data[keys[i]].horaInicioEvento);
             var horaIniSpt=horaIni.split(':');
             var horaIniSplit=horaIniSpt[0];            
             var minIniSplit=horaIniSpt[1];
             var fechaFin=(data[keys[i]].fechaEventoFin);
             var fechaFinSplit=fechaFin.split('/');
             var diaFinSplit=fechaFinSplit[0];
             var mesFinSplit=fechaFinSplit[1];
             var anyoFinSplit=fechaFinSplit[2];
             var horaFin=(data[keys[i]].horaFinEvento);
             var horaFinSpt=horaFin.split(':');
             var horaFinSplit=horaFinSpt[0];
             var minFinSplit=horaFinSpt[1];
             var Month=fechaActual.getMonth();  
             var hin= parseInt(horaIniSplit)+2;
             var hfi=parseInt(horaFinSplit)+2;         
             var dateActual=new Date(fechaActual.getFullYear(),Month,fechaActual.getDate(),fechaActual.getHours()+2,fechaActual.getMinutes(),fechaActual.getSeconds());
             var dateInitEvent=new Date(anyoIniSplit,mesIniSplit-1,diaIniSplit,hin,minIniSplit,0);
             var dateFinEvent=new Date(anyoFinSplit,mesFinSplit-1,diaFinSplit,hfi,minFinSplit,0);
            
             if (dateActual>=dateInitEvent&&dateActual<=dateFinEvent)
             {
              eventoName.push(data[keys[i]].nombreEvento);
              eventoID.push(data[keys[i]].idEvento);
             }
           }
        }      
    });
    db.ref('Zonas').once('value', (snapshot)=>{
        var data2 = snapshot.val();
        var keys2=Object.keys(data2);
        var fechaActual2=new Date;
        var jsonZonas=[];
        for(var l=0; l<eventoID.length; l++){
          
          for(var g=0; g<keys2.length;g++){
            if(eventoID[l]==data2[keys2[g]].idEventoZona&&data2[keys2[g]].zonaActiva=="Si"&&data2[keys2[g]].zonaBloqueada=="No"){
              var fechaFinZona=data2[keys2[g]].finZona;
              var splFechaHoraZona=fechaFinZona.split(' ');
              var fechaFinZonaSp=splFechaHoraZona[0].split('/');
              var horasFinZonaSp=splFechaHoraZona[1].split(':');
              
              var dateFinZona=new Date(fechaFinZonaSp[0],parseInt(fechaFinZonaSp[1])-1,fechaFinZonaSp[2],parseInt(horasFinZonaSp[0])+2,horasFinZonaSp[1],horasFinZonaSp[2]);
              var dateActual2=new Date(fechaActual2.getFullYear(), fechaActual2.getMonth(),fechaActual2.getDate(),fechaActual2.getHours()+2,fechaActual2.getMinutes(),fechaActual2.getSeconds() );
              if (dateFinZona>dateActual2){
                jsonZonas.push({
                  nombreEvento: eventoName[l],
                  nombreZona: data2[keys2[g]].nombreZona ,
                  idZona: data2[keys2[g]].idZona,
                  finZona:data2[keys2[g]].finZona
                });
              }             
            }             
          }
          
        }        
      res.render('index', {Zonas: jsonZonas});
    });
});

router.post('/new-contact', (req, res)=>{
     //Obtienes la fecha
     var dat= new Date();
     var month=parseInt(dat.getMonth())+1;
    console.log(req.body);
    const newRegister = {
        idZona: req.body.zoneSelected,
        firstname: req.body.firstname,
        lastname: req.body.lastname,
        age: req.body.age,
        identityCard: req.body.identityCard,
        timeIn:dat.getFullYear()+'/'+month+'/'+dat.getDate()+' '+ dat.getHours() +':'+dat.getMinutes()+':'+dat.getSeconds(),
        timeOut:""
       
    };
    db.ref('registrados').push(newRegister);
    res.redirect('/');
});

router.get('/delete-contact/:id', (req, res)=>{
    db.ref('registrados/'+req.params.id).remove();
    res.redirect('/');
});

router.get('/Eventos', (req, res)=>{
    reference=db.ref("Eventos");
    reference.once("value", function(snapshot){
        
        return res.json(snapshot.val());
      }, function(errorObject) {
        console.log("The read failed: " + errorObject.code);
      });
});

router.get('/Zonas', (req, res)=>{
    reference=db.ref("Zonas");
    reference.once("value", function(snapshot){
        
        return res.json(snapshot.val());
      }, function(errorObject) {
        console.log("The read failed: " + errorObject.code);
      });
});
router.get('/registrados', (req, res)=>{
  reference=db.ref("registrados");
  reference.once("value", function(snapshot){
      
      return res.json(snapshot.val());
    }, function(errorObject) {
      console.log("The read failed: " + errorObject.code);
    });
});

router.put('/Image', (req,res)=>{
    const filePath=req.body.imagen; 
    
    async function uploadFile() {

        let udss=uuid2();
        const metadata = {
          metadata: {         
            firebaseStorageDownloadTokens: udss           
          },
          contentType: 'image/png',         
          cacheControl: 'public, max-age=31536000',
        };             
        return await bucket.upload(filePath, {          
          gzip: true,
          metadata: metadata,              
        }).then(datos =>{   
          let file=datos[0];  
          res.send("https://firebasestorage.googleapis.com/v0/b/" + bucket.name + "/o/" + encodeURIComponent(file.name) + "?alt=media&token=" + udss);                  
        }) ;      
      }      
      uploadFile().catch(console.error);
});

 router.post('/Eventos/:id', (req,res)=>{
    const nombreEvento=req.body.nombreEvento;
    const direccionEvento=req.body.direccionEvento;
    const activoEvento=req.body.activoEvento;
    const idEvento=req.body.idEvento;
    const fechaEvento=req.body.fechaEvento;
    const horaInicioEvento=req.body.horaInicioEvento;
    const horaFinEvento=req.body.horaFinEvento;
    const zonasEvento=req.body.zonasEvento;
    const aforoEvento=req.body.aforoEvento;
    const fotoEvento=req.body.fotoEvento;
    const fechaEventoFin=req.body.fechaEventoFin;
    const ids=req.params.id;
    const nuevoEvento=db.ref('Eventos').child(ids);
    nuevoEvento.update({
        nombreEvento:nombreEvento,
        direccionEvento:direccionEvento,
        activoEvento:activoEvento,
        idEvento:idEvento,
        fechaEvento:fechaEvento,
        horaInicioEvento:horaInicioEvento,
        horaFinEvento:horaFinEvento,
        zonasEvento:zonasEvento,
        aforoEvento:aforoEvento,
        fotoEvento:fotoEvento,
        fechaEventoFin:fechaEventoFin
    });
    res.writeHead(200, {"Content-Type": "application/json"});
  res.end(JSON.stringify());
 });           

router.post('/Zonas/:id',(req,res)=>{
  const nombreZona=req.body.nombreZona;
  const metrosCuadradosZona=req.body.metrosCuadradosZona;
  const idZona=req.body.idZona;
  const aforoZona=req.body.aforoZona;
  const idEventoZona=req.body.idEventoZona;
  const topPositionZona=req.body.topPositionZona;
  const leffPositionZona=req.body.leffPositionZona;
  const heightZona=req.body.heightZona;
  const widthZona=req.body.widthZona;
  const zonaActiva=req.body.zonaActiva;
  const zonaDibujada=req.body.zonaDibujada;
  const situacionZona=req.body.situacionZona;
  const zonaBloqueada=req.body.zonaBloqueada;
  const inicioZona=req.body.inicioZona;
  const finZona=req.body.finZona;
  const ids=req.params.id;
  const nuevaZona=db.ref('Zonas').child(ids);
  nuevaZona.update({
      nombreZona : nombreZona,
      metrosCuadradosZona : metrosCuadradosZona,
      idZona : idZona,
      aforoZona : aforoZona,
      idEventoZona : idEventoZona,
      topPositionZona : topPositionZona,
      leffPositionZona : leffPositionZona,
      heightZona : heightZona,
      widthZona : widthZona,
      zonaActiva : zonaActiva,
      zonaDibujada : zonaDibujada,
      situacionZona : situacionZona,
      zonaBloqueada:zonaBloqueada,
      inicioZona:inicioZona,
      finZona:finZona
  });
  res.writeHead(200, {"Content-Type": "application/json"});
  res.end(JSON.stringify());
});

router.put('/Eventos/:id', (req,res)=>{
  const activoEvento=req.body.activoEvento;
  const ids=req.params.id;
    const altaEvento=db.ref('Eventos').child(ids);
    altaEvento.update({
      activoEvento:activoEvento
    });
    res.writeHead(200, {"Content-Type": "application/json"});
  res.end(JSON.stringify());
});

router.put('/Zonas/:id', (req,res)=>{
  const zonaActiva=req.body.zonaActiva;
  const ids=req.params.id;
  const activaZona=db.ref('Zonas').child(ids);
  activaZona.update({
    zonaActiva:zonaActiva
  });
res.writeHead(200, {"Content-Type": "application/json"});
  res.end(JSON.stringify());
});

router.put('/Zonas-bloqueo/:id', (req,res)=>{
  const zonaBloqueada=req.body.zonaBloqueada;
  const ids=req.params.id;
  const activaZona=db.ref('Zonas').child(ids);
  activaZona.update({
    zonaBloqueada:zonaBloqueada
  });
res.writeHead(200, {"Content-Type": "application/json"});
  res.end(JSON.stringify());
});

 router.put('/registrados/:id', (req,res)=>{
  const timeOut=req.body.timeOut;
  const ids=req.params.id;
  const activaZona=db.ref('registrados').child(ids);
  activaZona.update({
    timeOut:timeOut
  });
res.writeHead(200, {"Content-Type": "application/json"});
  res.end(JSON.stringify());
});
router.post('/delete-zonas/:id', (req, res) => {
  db.ref('Zonas/' + req.params.id).remove();
  res.writeHead(200, {"Content-Type": "application/json"});
  res.end();
});

module.exports = router;