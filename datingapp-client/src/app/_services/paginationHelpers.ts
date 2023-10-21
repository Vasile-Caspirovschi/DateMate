import { HttpClient, HttpParams } from "@angular/common/http";
import { map } from "rxjs";
import { PaginatedResult } from "../_models/pagination";

export function getPaginationResult<T>(url: string, params: HttpParams, http: HttpClient) {
    const paginatedResult = new PaginatedResult<T>();
    return http.get<T>(url, { observe: 'response', params }).pipe(
        map(response => {
            if (response.body)
                paginatedResult.result = response.body;
            const paginationHeader = response.headers.get('Pagination');
            if (paginationHeader)
                paginatedResult.pagination = JSON.parse(paginationHeader);
            return paginatedResult;
        })
    );
}

export function getPaginationHeaders(pageNumber: number, pageSize: number) {
    let params = new HttpParams();
    params = params.append('pageNumber', pageNumber.toString());
    params = params.append('pageSize', pageSize.toString());
    return params;
}