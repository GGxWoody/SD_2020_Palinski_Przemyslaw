import { Component, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { User } from 'src/app/_models/user';
import { UserService } from 'src/app/_services/user.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { ActivatedRoute } from '@angular/router';
import { BsModalRef, BsModalService, TabsetComponent } from 'ngx-bootstrap';
import { AuthService } from 'src/app/_services/auth.service';
import { FriendInviteService } from 'src/app/_services/friend-invite.service';
import { Team } from 'src/app/_models/team';

@Component({
  selector: 'app-member-detail',
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.scss']
})
export class MemberDetailComponent implements OnInit {
  user: User;
  loginedInUser: User;
  modalRef: BsModalRef;
  teamSelected: Team;
  inTeam: boolean;
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

  constructor(private authService: AuthService, private userService: UserService,
              private alertify: AlertifyService, private root: ActivatedRoute,
              private inviteService: FriendInviteService, private modalService: BsModalService) { }

  ngOnInit() {
    this.root.data.subscribe(data => {
      this.user = data.user;
      this.loginedInUser = data.loginedInUser;
    });
  }

  openModal(template: TemplateRef<any>) {
    this.modalRef = this.modalService.show(template);
  }

  sendFriendInvite(id: number) {
    this.inviteService.sendFriendInvite(this.authService.decodedToken.nameid, id).subscribe(data => {
      this.alertify.success('You invited: ' + this.user.knownAs + ' to friendlist');
    }, error => {
      this.alertify.error(error);
    });
  }

  selectionChanged() {
    if (this.user.teams.find(x => x.id === this.teamSelected.id) != null) {
      this.inTeam = true;
    } else {
      this.inTeam = false;
    }
  }

  sendTeamInvite(id: number) {
    this.inviteService.sendTeamInvite(this.authService.decodedToken.nameid, this.teamSelected.id, id).subscribe(data => {
      this.alertify.success('You invited: ' + this.user.knownAs + ' to friendlist');
    }, error => {
      this.alertify.error(error);
    });
  }
}
