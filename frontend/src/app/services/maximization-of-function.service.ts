import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { MOFPost, MOFResponse } from "../models/maximization-of-function";

@Injectable()
export class MOFService {

    url: string = "http://localhost:5000/maximization-of-function";

    headers = new HttpHeaders({
        'Content-Type': 'application/json',
        'Access-Control-Allow-Origin': '*',
        'Access-Control-Allow-Headers': 'Content-Type',
        'Access-Control-Allow-Methods': 'GET,POST,OPTIONS,DELETE,PUT',
        'Authorization': 'Bearer szdp79a2kz4wh4frjzuqu4sz6qeth8m3',
     });


    constructor(private http: HttpClient) { }

    post(postData: MOFPost){
        return this.http.post<MOFResponse>(this.url, postData, {headers: this.headers});
    }
}