import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators, FormGroup, FormArray, FormControl } from '@angular/forms';
import { AdminLinesService } from './admin-lines.service';
import { from } from 'rxjs';
import { TouchSequence } from 'selenium-webdriver';


@Component({
  selector: 'app-admin-lines',
  templateUrl: './admin-lines.component.html',
  styleUrls: ['./admin-lines.component.css']
})
export class AdminLinesComponent implements OnInit {

  stations : string[];
  selectedStations: string = "";
  addMssg : string;
  deleteMssg : string;
  stationMssg : string;
  type : string = "";
  lines: any = [];
  deleteLine: any = [];
  selectedLine: string;
  showStations: string;
  addStation: string;

  Form = this.fb.group({
    name: ['',Validators.required],
    stations : ['',Validators.required],
  });

  

  constructor(private formBuilder: FormBuilder, private service: AdminLinesService, private fb : FormBuilder) {

  }

  select(event : any)
  {
    this.type = event.target.value;
  }

  selectDelete(event : any)
  {
    this.deleteLine = event.target.value;
    this.lines.forEach(element => {
      if(element.Line == this.deleteLine)
      {
        this.deleteLine = element;
        
      }
    });
  }
  
 
  ngOnInit() {
    this.service.GetStations().subscribe((data)=> {
      this.stations = data;
    });
    this.service.GetLines().subscribe((data)=> {
     this.lines = data;   
     });
  }


  selectedStation(event: any) {
    //update the ui
    if(event.target.value != "")
    {
       this.selectedStations += event.target.value + ",";
    }
  }

  selectedStationAdd(event: any) {
    //update the ui
    if(event.target.value != "")
    {
       this.addStation = event.target.value;
    }
  }

AddLine()
{
  this.service.AddLine(this.Form.value.name,this.selectedStations,this.type).subscribe((data)=>{
      this.addMssg = data;
      this.service.GetLines().subscribe((data)=> {
        this.lines = data;
      });
  });
  
}

Delete()
{
  
  this.service.RemoveLine(this.deleteLine).subscribe((data)=>{
    this.deleteMssg = data;
    this.service.GetLines().subscribe((data)=> {
      this.lines = data;
    });
  });
}

Show()
{
  this.service.ShowLine(this.deleteLine.Line).subscribe((data)=>{
    this.showStations = data;
    
  });
}

AddStation()
{
  this.service.AddStation(this.deleteLine,this.addStation).subscribe((data)=>{
    this.stationMssg = data;
    
  });
}

DeleteStation()
{
  this.service.DeleteStation(this.deleteLine,this.addStation).subscribe((data)=>{
    this.stationMssg = data;
    
  });
}


}
