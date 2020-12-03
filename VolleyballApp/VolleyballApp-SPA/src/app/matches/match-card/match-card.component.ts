import { Component, Input, OnInit } from '@angular/core';
import { Match } from 'src/app/_models/match';
import { User } from 'src/app/_models/user';

@Component({
  selector: 'app-match-card',
  templateUrl: './match-card.component.html',
  styleUrls: ['./match-card.component.scss']
})
export class MatchCardComponent implements OnInit {
  @Input() match: Match;
  user: User = JSON.parse(localStorage.getItem('user'));
  constructor() { }

  ngOnInit() {
  }

}
