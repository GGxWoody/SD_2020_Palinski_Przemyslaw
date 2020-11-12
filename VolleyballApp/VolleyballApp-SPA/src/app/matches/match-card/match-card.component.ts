import { Component, Input, OnInit } from '@angular/core';
import { Match } from 'src/app/_models/match';

@Component({
  selector: 'app-match-card',
  templateUrl: './match-card.component.html',
  styleUrls: ['./match-card.component.scss']
})
export class MatchCardComponent implements OnInit {
  @Input() match: Match;
  constructor() { }

  ngOnInit() {
  }

}
