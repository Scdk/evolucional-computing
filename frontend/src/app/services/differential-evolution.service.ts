import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { DifEvoPost, DifEvoResponse } from "../models/differential-evolution";

@Injectable()
export class DifEvoService {

    url: string = "http://localhost:5000/differential-evolution";

    headers = new HttpHeaders({
        'Content-Type': 'application/json',
        'Access-Control-Allow-Origin': '*',
        'Access-Control-Allow-Headers': 'Content-Type',
        'Access-Control-Allow-Methods': 'GET,POST,OPTIONS,DELETE,PUT',
        'Authorization': 'Bearer szdp79a2kz4wh4frjzuqu4sz6qeth8m3',
     });


    constructor(private http: HttpClient) { }

    post(postData: DifEvoPost){
        return this.http.post<DifEvoResponse>(this.url, postData, {headers: this.headers});
    }
}