import { Component, Input, OnInit } from '@angular/core';
import { League } from 'src/app/_models/league';

@Component({
  selector: 'app-leagues-card',
  templateUrl: './leagues-card.component.html',
  styleUrls: ['./leagues-card.component.scss']
})
export class LeaguesCardComponent implements OnInit {
  @Input() league: League;
  constructor() { }

  ngOnInit() {
  }

}
