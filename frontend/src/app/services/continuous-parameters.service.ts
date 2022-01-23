import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { ContParamsPost, ContParamsResponse } from "../models/continuous-parameters";

@Injectable()
export class ContParamsService {

    url: string = "http://localhost:5000/continuous-parameters";

    headers = new HttpHeaders({
        'Content-Type': 'application/json',
        'Access-Control-Allow-Origin': '*',
        'Access-Control-Allow-Headers': 'Content-Type',
        'Access-Control-Allow-Methods': 'GET,POST,OPTIONS,DELETE,PUT',
        'Authorization': 'Bearer szdp79a2kz4wh4frjzuqu4sz6qeth8m3',
     });


    constructor(private http: HttpClient) { }

    post(postData: ContParamsPost){
        return this.http.post<ContParamsResponse>(this.url, postData, {headers: this.headers});
    }
}