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
  @ViewChild('memberTabs', {static: true}) memberTabs: TabsetComponent;
  user: User;
  loginedInUser: User;
  modalRef: BsModalRef;
  inTeam: boolean;

  constructor(private authService: AuthService, private userService: UserService,
              private alertify: AlertifyService, private root: ActivatedRoute,
              private inviteService: FriendInviteService) { }

  ngOnInit() {
    this.root.data.subscribe(data => {
      this.user = data.user;
      this.loginedInUser = data.loginedInUser;
    });

    this.root.queryParams.subscribe(params => {
      const selectedTab = params.tab;
      this.memberTabs.tabs[selectedTab > 0 ? selectedTab : 0].active = true;
    });
  }

  sendFriendInvite() {
    this.inviteService.sendFriendInvite(this.authService.decodedToken.nameid, this.user.id).subscribe(data => {
      this.alertify.success('You invited: ' + this.user.knownAs + ' to friendlist');
    }, error => {
      this.alertify.error(error);
    });
  }

  sendTeamInvite() {
    this.inviteService.sendTeamInvite(this.authService.decodedToken.nameid, this.loginedInUser.team.id, this.user.id)
    .subscribe(data => {
      this.alertify.success('You invited: ' + this.user.knownAs + ' to your team');
    }, error => {
      this.alertify.error(error);
    });
  }

  selectTab(tabId: number) {
    this.memberTabs.tabs[tabId].active = true;
  }
}
