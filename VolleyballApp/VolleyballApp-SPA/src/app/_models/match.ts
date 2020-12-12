import { Score } from './score';
import { Team } from './team';
import { Location } from './location';
import { User } from './user';

export class Match {
    id: number;
    firstTeam: Team;
    secondTeam: Team;
    score?: Score;
    timeOfMatch?: Date;
    location?: Location;
    referee?: User;
    isRefereeInvited?: boolean;
}
