import { Score } from './score';
import { Team } from './team';

export class Match {
    id: number;
    firstTeam: Team;
    secondTeam: Team;
    score?: Score;
}
