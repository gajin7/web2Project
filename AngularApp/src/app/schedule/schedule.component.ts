import { Component, OnInit } from '@angular/core';
import { ScheduleService } from './schedule.service';
import { Router } from '@angular/router';
import { FormBuilder } from '@angular/forms';

@Component({
  selector: 'app-schedule',
  templateUrl: './schedule.component.html',
  styleUrls: ['./schedule.component.css']
})
export class ScheduleComponent implements OnInit {

  lineType : string = "";
  lines : string[];
  constructor(public service: ScheduleService, private route: Router,private fb: FormBuilder) { }
  

  ngOnInit() {
  }

  selected (event: any) {
    //update the ui
    this.lineType = event.target.value;
    this.GetLines();
   
  }


  GetLines()
  {
    this.service.GetLines(this.lineType).subscribe((data) => { 
     this.lines = data; });

      
  }

}
