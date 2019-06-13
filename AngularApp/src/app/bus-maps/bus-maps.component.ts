import { Component, OnInit } from '@angular/core';
import { MarkerInfo } from './marker-info.model';
import { GeoLocation } from './geolocation';
import { Polyline } from './polyliner';


@Component({
  selector: 'app-bus-maps',
  templateUrl: './bus-maps.component.html',
  styleUrls: ['./bus-maps.component.css'],
  styles: ['agm-map {height: 500px; width: 700px;}']
})
export class BusMapsComponent implements OnInit {
  markerInfo: MarkerInfo;
  selLine: Polyline;
  constructor() { }

  ngOnInit() {
    this.markerInfo = new MarkerInfo(new GeoLocation(45.242268, 19.842954), 
    "assets/ftn.png",
    "Jugodrvo" , "" , "http://ftn.uns.ac.rs/691618389/fakultet-tehnickih-nauka");
    this.selLine = new Polyline([], 'red', { url:"assets/busicon.png", scaledSize: {width: 50, height: 50}});
   
    
  }
}
