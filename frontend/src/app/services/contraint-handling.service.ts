import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { ConstHandPost, ConstHandResponse } from "../models/contraint-handling";

@Injectable()
export class ConstHandService {

    url: string = "http://localhost:5000/constraint-handling";

    headers = new HttpHeaders({
        'Content-Type': 'application/json',
        'Access-Control-Allow-Origin': '*',
        'Access-Control-Allow-Headers': 'Content-Type',
        'Access-Control-Allow-Methods': 'GET,POST,OPTIONS,DELETE,PUT',
        'Authorization': 'Bearer szdp79a2kz4wh4frjzuqu4sz6qeth8m3',
     });


    constructor(private http: HttpClient) { }

    post(postData: ConstHandPost){
        return this.http.post<ConstHandResponse>(this.url, postData, {headers: this.headers});
    }
}