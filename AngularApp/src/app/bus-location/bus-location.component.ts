import { Component, OnInit, NgZone } from '@angular/core';
import { FormGroup, FormBuilder, FormArray, FormControl } from '@angular/forms';
import { MarkerInfo } from '../bus-maps/marker-info.model';
import { Polyline } from '../bus-maps/polyliner';
import { BusMapsService } from '../bus-maps/bus-maps.service';
import { GeoLocation } from '../bus-maps/geolocation';
import { StationModel } from '../bus-maps/stationModel';
import { MapsAPILoader } from '@agm/core';
import { NotificationsForBusLocService } from './notification-for-bus-loc.service';
import { ForBusLocationService } from './for-bus-location.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-bus-location',
  templateUrl: './bus-location.component.html',
  styleUrls: ['./bus-location.component.css'],
  styles: ['agm-map {height: 500px; width: 1000px;}']
})
export class BusLocationComponent implements OnInit {

  markerInfo: MarkerInfo;
  selLine: Polyline;
  stati: any = [];
  showLines: any =[];
  allLines: any = [];
  showStations  = false;

  myGroup: FormGroup;
  isChanged : boolean = false;
  show: boolean = false;
  iconPath : any = { url:"assets/images/autobus.png", scaledSize: {width: 35, height: 35}}
  public polyline: Polyline;
  public polylineRT: Polyline;  
  public zoom: number = 15;
  startLat : number = 45.242268;
  startLon : number = 19.842954;

  options : string[];
  options1: any;
  stations : StationModel[] = [];
  buses : any[];
  busImgIcon : any = {url:"assets/images/autobus.png", scaledSize: {width: 50, height: 50}};
  autobusImgIcon : any = {url:"assets/images/busicon.png", scaledSize: {width: 50, height: 50}};

  isConnected: boolean;
  notifications: string[];
  time: number[] = [];

  latitude : number ;
  longitude : number;
  marker: MarkerInfo = new MarkerInfo(new GeoLocation(this.startLat,this.startLon),"","","","");

  constructor(public service : BusMapsService,  private route: Router, private formBuilder: FormBuilder,private mapsApiLoader : MapsAPILoader,private notifForBL : NotificationsForBusLocService, private ngZone: NgZone, private clickService : ForBusLocationService) {
    this.isConnected = false;
    this.notifications = [];
   }

  ngOnInit() {
    this.markerInfo = new MarkerInfo(new GeoLocation(45.242268, 19.842954), 
    "assets/images/ftn.png",
    "Jugodrvo" , "" , "http://ftn.uns.ac.rs/691618389/fakultet-tehnickih-nauka");
    this.selLine = new Polyline([], 'red', { url:"assets/images/busicon.png", scaledSize: {width: 50, height: 50}});
    this.GetAllLines();
    this.isChanged = false;
    //za combobox izlistaj sve linije
    this.clickService.getAllLines().subscribe(
      data =>{
        this.options = [];
        this.options1 = data;
        this.options1.forEach(element => {
          this.options.push(element.LineNumber);
        });
      });
    //inicijalizacija polyline
    this.polyline = new Polyline([], 'blue', { url:"assets/images/autobus.png", scaledSize: {width: 50, height: 50}});

    this.checkConnection();
    this.subscribeForTime();
    this.stations = [];
  }

  GetAllStations()
  {
      this.service.GetAllStations().subscribe((data)=>{
         this.stati = data;
      });
  }

  GetAllLines()
  {
    this.service.GetAllLines().subscribe((data)=>{
        this.allLines = data;
    });
  }


  ShowStationsSelectionChanged()
  {
    if(this.showStations == false)
      {
        this.GetAllStations();
        this.showStations = true;
      }
    else
    {
      this.stati = [];
      this.showStations = false;
    }
  }

  onSelectionChangeNumber(event){
    this.isChanged = true;
    this.stations = [];
    this.polyline.path = [];
    if(event.target.value == "")
    {
      this.isChanged = false;
      this.stations = [];
      this.polyline.path = [];
      this.stopTimer();
    }else
    {
      this.stopTimer();
      this.getStationsByLineNumber(event.target.value);   
    
    //  this.notifForBL.StartTimer(); 
    }
    
  }

  getStationsByLineNumber(lineNumber : string){
    this.options1.forEach(element => {
      if(element.LineNumber == lineNumber)
      {
        this.stations = element.Stations;
        for(var i=0; i<this.stations.length; ++i){
          this.polyline.addLocation(new GeoLocation(this.stations[i].Latitude, this.stations[i].Longitude));
        }
        console.log(this.stations);
        this.clickService.click(this.stations).subscribe(data =>
          {
            this.startTimer();
          });
      }
    });
  }
  
  private checkConnection(){
    this.notifForBL.startConnection().subscribe(e => {
      this.isConnected = e; 
       
      // this.notifForBL.StartTimer();
        
    });
  }  

 public subscribeForTime() {
    this.notifForBL.registerForTimerEvents().subscribe(e => this.onTimeEvent(e));
  }

  public onTimeEvent(pos: number[]){
    this.ngZone.run(() => { 
       this.time = pos; 
       if(this.isChanged){
         this.latitude = pos[0];
          this.longitude = pos[1];

       }else{
          this.latitude = 0;
          this.longitude = 0;
       }
    });      
  }  

  public startTimer() {    
    this.notifForBL.StartTimer();
  }

  public stopTimer() {
    this.notifForBL.StopTimer();
    this.time = null;
  }

  Navigate()
  {
    if(localStorage.role == "AppUser")
    {
      this.route.navigate(['app-user-home']);
    }
    else
    {
      this.route.navigate(['home']);
    }
    
  }


}

