import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { IArticle } from '../Dto/iarticle.dto';

@Injectable({
  providedIn: 'root'
})
export class ProductService {

  private apiUrl ='https://localhost:7015/api/Products';

  //https://flapotest.blob.core.windows.net/test/ProductData.json

  //https://localhost:7015/api/Products

  constructor(private _http : HttpClient) { }

  getProducts(sort?: 'asc' | 'desc', filterExpensive?: boolean): Observable<IArticle[]> {
    let params = new HttpParams();
    if (sort) params = params.set('sort', sort);
    if (filterExpensive) params = params.set('cheaperThan2PerLiter', 'true');
    return this._http.get<IArticle[]>(this.apiUrl, { params });
  }
  
}
