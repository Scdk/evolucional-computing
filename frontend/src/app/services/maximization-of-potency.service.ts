import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { MaxPotPost, MaxPotResponse } from "../models/maximization-of-potency";

@Injectable()
export class MaxPotService {

    url: string = "http://localhost:5000/maximization-of-potency";

    headers = new HttpHeaders({
        'Content-Type': 'application/json',
        'Access-Control-Allow-Origin': '*',
        'Access-Control-Allow-Headers': 'Content-Type',
        'Access-Control-Allow-Methods': 'GET,POST,OPTIONS,DELETE,PUT',
        'Authorization': 'Bearer szdp79a2kz4wh4frjzuqu4sz6qeth8m3',
     });


    constructor(private http: HttpClient) { }

    post(postData: MaxPotPost){
        return this.http.post<MaxPotResponse>(this.url, postData, {headers: this.headers});
    }
}