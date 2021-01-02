import { ViewChild } from '@angular/core';
import { Component, OnInit, TemplateRef } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { BsModalRef, BsModalService } from 'ngx-bootstrap';
import { Team } from 'src/app/_models/team';
import { User } from 'src/app/_models/user';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { AuthService } from 'src/app/_services/auth.service';
import { FriendInviteService } from 'src/app/_services/friend-invite.service';
import { TeamService } from 'src/app/_services/team.service';

@Component({
  selector: 'app-team-detail',
  templateUrl: './team-detail.component.html',
  styleUrls: ['./team-detail.component.css']
})
export class TeamDetailComponent implements OnInit {
  @ViewChild('editForm', { static: true }) editForm: NgForm;
  team: Team;
  userList: User[];
  loginedInUser: User;
  teamSelected: Team;
  modalRef: BsModalRef;
  config = {
    displayKey: 'teamName',
    search: true,
    height: 'auto',
    placeholder: 'Select team',
    noResultsFound: 'No teams found!',
    searchPlaceholder: 'Search team',
    searchOnKey: 'teamName',
    clearOnSelection: true
  };

  constructor(private root: ActivatedRoute, private modalService: BsModalService, private inviteService: FriendInviteService,
              private alertify: AlertifyService, private authService: AuthService, private teamService: TeamService) { }

  ngOnInit() {
    this.root.data.subscribe(data => {
      this.team = data.team;
      this.loginedInUser = data.loginedInUser;
    });
    this.userList = this.team.userTeams.map(x => x.user);
  }

  openModal(template: TemplateRef<any>) {
    this.modalRef = this.modalService.show(template);
  }

  sendMatchInvite() {
    this.inviteService.sendMatchInvite(this.loginedInUser.id, this.loginedInUser.userTeam.teamId , this.team.id).subscribe(data => {
      this.alertify.success('You invited: ' + this.team.teamName + ' to play match');
    }, error => {
      this.alertify.error(error);
    });
  }

  updateTeam() {
    this.teamService.updateTeam(this.team.id, this.team).subscribe(next => {
      this.alertify.success('Profile updated successfully');
    }, error => {
      this.alertify.error(error);
    });
  }
}
