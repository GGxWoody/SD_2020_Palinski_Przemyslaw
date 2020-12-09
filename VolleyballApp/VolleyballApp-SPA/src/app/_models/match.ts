import { Score } from './score';
import { Team } from './team';
import { Location } from './location';

export class Match {
    id: number;
    firstTeam: Team;
    secondTeam: Team;
    score?: Score;
    timeOfMatch?: Date;
    location?: Location;
}
