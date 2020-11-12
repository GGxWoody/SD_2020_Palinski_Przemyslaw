import { Match } from './match';
import { Set } from './set';

export class Score {
    id: number;
    match: Match;
    firstTeamSets: number;
    secondTeamSets: number;
    setList?: Set[];
}
