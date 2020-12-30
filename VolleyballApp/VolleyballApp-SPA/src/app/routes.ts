import { Routes } from '@angular/router';
import { ActivateLinkComponent } from './activate-link/activate-link.component';
import { ActivateComponent } from './activate/activate.component';
import { HomeComponent } from './home/home.component';
import { InviteListComponent } from './invites/invite-list/invite-list.component';
import { LeagueDetailComponent } from './leagues/league-detail/league-detail.component';
import { LeaguesListComponent } from './leagues/leagues-list/leagues-list.component';
import { MatchDetailComponent } from './matches/match-detail/match-detail.component';
import { MatchListComponent } from './matches/match-list/match-list.component';
import { MemberDetailComponent } from './members/member-detail/member-detail.component';
import { MemberEditComponent } from './members/member-edit/member-edit.component';
import { MemberFriendListComponent } from './members/member-friend-list/member-friend-list.component';
import { MemberListComponent } from './members/member-list/member-list.component';
import { MessagesComponent } from './messages/messages.component';
import { TeamDetailComponent } from './teams/team-detail/team-detail.component';
import { TeamListComponent } from './teams/team-list/team-list.component';
import { AuthGuard } from './_guards/auth.guard';
import { PreventUnsavedChanges } from './_guards/prevent-unsaved-changes.guard';
import { FriendListResolver } from './_resolvers/friend-list.resolver';
import { InviteListResolver } from './_resolvers/invite-list.resolver';
import { LeagueDetailResolver } from './_resolvers/league-detail.resolver';
import { LeagueListResolver } from './_resolvers/league-list.resolver';
import { MatchDetailResolver } from './_resolvers/match-detail.resolver';
import { MatchListResolver } from './_resolvers/match-list.resolver';
import { MatchRefereeListResolver } from './_resolvers/match-referee-list.resolver';
import { MatchesForLeagueResolver } from './_resolvers/matches-for-league.resolver';
import { MemberDetailResolver } from './_resolvers/member-detail.resolver';
import { MemberEditResolver } from './_resolvers/member-edit.resolver';
import { MemberListResolver } from './_resolvers/member-list.resolver';
import { MessagesResolver } from './_resolvers/messages.resolver';
import { TeamDetailResolver } from './_resolvers/team-detail.resolver';
import { TeamListResolver } from './_resolvers/team-list.resolver';

export const appRoutes: Routes = [
    { path: '', component: HomeComponent },
    { path: 'activate/:id', component: ActivateLinkComponent},
    {
        path: '',
        runGuardsAndResolvers: 'always',
        canActivate: [AuthGuard],
        children: [
            {
                path: 'members/edit', component: MemberEditComponent, resolve: { user: MemberEditResolver },
                canDeactivate: [PreventUnsavedChanges]
            },
            { path: 'activate', component: ActivateComponent},
            { path: 'members', component: MemberListComponent, resolve: { users: MemberListResolver } },
            { path: 'friends', component: MemberFriendListComponent, resolve: { friends: FriendListResolver } },
            { path: 'teams', component: TeamListComponent, resolve: {teams: TeamListResolver} },
            { path: 'matches', component: MatchListComponent, resolve: {matches: MatchListResolver} },
            { path: 'leagues', component: LeaguesListComponent, resolve: {leagues: LeagueListResolver} },
            { path: 'invites', component: InviteListComponent, resolve: {invites: InviteListResolver} },
            { path: 'messages', component: MessagesComponent, resolve: {messages: MessagesResolver} },
            { path: 'members/:id', component: MemberDetailComponent,
             resolve: { user: MemberDetailResolver, loginedInUser: MemberEditResolver } },
            { path: 'leagues/:id', component: LeagueDetailComponent,
            resolve: {league: LeagueDetailResolver, matches: MatchesForLeagueResolver } },
            { path: 'teams/:id', component: TeamDetailComponent,
            resolve: { team: TeamDetailResolver, loginedInUser: MemberEditResolver} },
            { path: 'matches/:id', component: MatchDetailComponent,
            resolve: { match: MatchDetailResolver, referees: MatchRefereeListResolver } }
        ]
    },
    { path: '**', redirectTo: '', pathMatch: 'full' },
];
