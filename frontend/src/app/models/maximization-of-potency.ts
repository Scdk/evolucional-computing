export interface MaxPotPost {
    populationSize: number;
	crossoverRate: number;
    mutationRate: number;
	numberOfGenerations: number;
}

export interface MaxPotResponse {
    generation: number;
	fittestIndividualFitness: number;
	generationAvarageFitness: number;
	scheduling: string[];
	liquidPotencies: number[];
	fittestIndividualStandardDeviation: number;
}