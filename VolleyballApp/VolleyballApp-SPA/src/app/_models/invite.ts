import { Team } from './team';
import { User } from './user';

export interface Invite {
    id: number;
    inviteFrom?: User;
    inviteTo?: User;
    teamInvited?: Team;
    friendInvite: boolean;
    teamInvite: boolean;
    matchInvite: boolean;
}
