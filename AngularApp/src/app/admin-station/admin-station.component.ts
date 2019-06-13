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

  addMssg : string;
  deleteMssg : string;

  constructor(private fb: FormBuilder, private service: AdminStationService) { }

  ngOnInit() {
  }

  AddStation()
  {
    this.service.AddStation(this.AddForm.value).subscribe((data)=>{
      this.addMssg = data;
    });
  }

  DeleteStation()
  {
    this.service.DeleteStation(this.deleteForm.value.id).subscribe((data)=>{
      this.deleteMssg = data;
    });
  }

}
