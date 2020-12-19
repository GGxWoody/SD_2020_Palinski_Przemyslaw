import { Component, OnInit } from '@angular/core';
import { ActivatedRoute} from '@angular/router';
import { League } from 'src/app/_models/league';
import { Match } from 'src/app/_models/match';
import { PaginatedResult, Pagination } from 'src/app/_models/pagination';
import { User } from 'src/app/_models/user';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { LeaguesService } from 'src/app/_services/leagues.service';

@Component({
  selector: 'app-league-detail',
  templateUrl: './league-detail.component.html',
  styleUrls: ['./league-detail.component.scss']
})
export class LeagueDetailComponent implements OnInit {
  user: User = JSON.parse(localStorage.getItem('user'));
  league: League;
  matches: Match[];
  pagination: Pagination;

  constructor(private leagueService: LeaguesService, private alertify: AlertifyService,
              private root: ActivatedRoute) { }

  ngOnInit() {
    this.root.data.subscribe(data => {
      this.league = data.league;
      this.matches = data.matches.result;
      this.pagination = data.matches.pagination;
    });

    this.league.teamLeague.sort((a, b) => b.leagueScore - a.leagueScore);
  }

  joinLeague() {
    this.leagueService.joinLeague(this.league.id, this.user.id).subscribe(data => {
      this.alertify.success('You Joined this league');
    }, error => {
      this.alertify.error(error);
    });
  }

  startLeague() {
    this.leagueService.startLeague(this.league.id).subscribe(data => {
      this.alertify.success('You started this league');
    }, error => {
      this.alertify.error(error);
    });
  }

  pageChanged(event: any): void {
    this.pagination.currentPage = event.page;
    this.loadMatches();
  }

  loadMatches() {
    this.leagueService.getLeagueMatches(this.league.id, this.pagination.currentPage, this.pagination.itemsPerPage)
    .subscribe((res: PaginatedResult<Match[]>) => {
      this.matches = res.result;
      this.pagination = res.pagination;
    }, error => {
      this.alertify.error(error);
    });
  }

}
