import { Component, OnInit } from '@angular/core';
import { Validators, FormBuilder } from '@angular/forms';
import { AdminStationService } from './admin-station.service';
import { MarkerInfo } from '../bus-maps/marker-info.model';
import { GeoLocation } from '../bus-maps/geolocation';
import { Polyline } from '../bus-maps/polyliner';

@Component({
  selector: 'app-admin-station',
  templateUrl: './admin-station.component.html',
  styleUrls: ['./admin-station.component.css'],
  styles: ['agm-map {height: 300px; width: 630px;}']
})
export class AdminStationComponent implements OnInit {

  AddForm = this.fb.group({
    name: ['',Validators.required],
    address: ['',Validators.required],
    longitude: ['',Validators.required],
    latitude: ['',Validators.required],
  });

  deleteForm = this.fb.group({
    id: ['',Validators.required]
  });

  changeForm = this.fb.group({
    name: ['',Validators.required],
    address: ['',Validators.required],
    longitude: ['',Validators.required],
    latitude: ['',Validators.required],
  });

  addMssg : string;
  deleteMssg : string;
  changeMssg : string;
  ids: any = [];
  id: string;
  markerInfo: MarkerInfo;
  selLine: Polyline;
  deleteStation : any = [];
  version :string;

  constructor(private fb: FormBuilder, private service: AdminStationService) { }

  ngOnInit() {
    this.service.GetStations().subscribe((data)=> {
      this.ids = data;
    });
    this.markerInfo = new MarkerInfo(new GeoLocation(45.242268, 19.842954), 
    "assets/images/ftn.png",
    "Jugodrvo" , "" , "http://ftn.uns.ac.rs/691618389/fakultet-tehnickih-nauka");
    this.selLine = new Polyline([], 'red', { url:"assets/images/autobus.png", scaledSize: {width: 50, height: 50}});
  }

  selected (event: any) {
    //update the ui
    this.id = event.target.value;
    this.service.GetStationInfo(this.id).subscribe((data)=>{
      this.changeForm.setValue({
        name : data.name,
        address : data.address,
        longitude: data.longitude,
        latitude : data.latitude
      });
    })

  }

  deleteSelected(event :any)
  {
    this.deleteForm.setValue(
      {
        id:event.target.value
      }
    );

    this.deleteStation= event.target.value;
    this.ids.forEach(element => {
      if(element.Station == this.deleteStation)
      {
        this.deleteStation = element;
        
      }
    });
  }

  MapClicked(event)
  {
    this.AddForm.setValue({
      name : "",
      address: "",
      longitude: event.coords.lng,
      latitude : event.coords.lat
    });
  }

  AddStation()
  {
    this.service.AddStation(this.AddForm.value).subscribe((data)=>{
      this.addMssg = data;
      this.service.GetStations().subscribe((data)=> {
        this.ids = data;
      });
    });
  }

  DeleteStation()
  {
    this.service.DeleteStation(this.deleteStation).subscribe((data)=>{
      this.deleteMssg = data;
      this.service.GetStations().subscribe((data)=> {
        this.ids = data;
      });
      this.service.GetStations().subscribe((data)=> {
        this.ids = data;
      });
    });
  }

  Change()
  {
    this.ids.forEach(element => {
      if(element.Station == this.id)
      {
        this.version = element.Version;
      }

    });
    this.service.ChangeStation(this.id,this.changeForm.value,this.version).subscribe((data)=>{
      this.changeMssg = data;
      this.service.GetStations().subscribe((data)=> {
        this.ids = data;
      });
    });
  }

}
