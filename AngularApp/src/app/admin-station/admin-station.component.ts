import { Component, OnInit } from '@angular/core';
import { Validators, FormBuilder } from '@angular/forms';
import { AdminStationService } from './admin-station.service';

@Component({
  selector: 'app-admin-station',
  templateUrl: './admin-station.component.html',
  styleUrls: ['./admin-station.component.css']
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
  ids: string[];
  id: string;

  constructor(private fb: FormBuilder, private service: AdminStationService) { }

  ngOnInit() {
    this.service.GetStations().subscribe((data)=> {
      this.ids = data;
    });
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
    this.service.DeleteStation(this.deleteForm.value.id).subscribe((data)=>{
      this.deleteMssg = data;
    });
  }

  Change()
  {
    this.service.ChangeStation(this.id,this.changeForm.value).subscribe((data)=>{
      this.changeMssg = data;
    });
  }

}
