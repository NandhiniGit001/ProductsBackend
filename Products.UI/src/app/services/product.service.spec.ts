import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { IArticle } from '../Dto/iarticle.dto'
import { ProductService } from './product.service';

describe('ProductService', () => {
  let service: ProductService;
  let httpMock: HttpTestingController;
  const apiUrl = 'https://localhost:7015/api/Products';

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [ProductService]
    });
    service = TestBed.inject(ProductService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  it('should fetch products with correct parameters', () => {
    const mockProducts: IArticle[] = [
      { id: 1, productName: 'Beer A', shortDescription: 'Description A', price: 10, unit: 'bottle', pricePerUnitText: '5€/L', image: 'image1.jpg' },
      { id: 2, productName: 'Beer B', shortDescription: 'Description B', price: 12, unit: 'bottle', pricePerUnitText: '6€/L', image: 'image2.jpg' },
    ];

    service.getProducts('asc', true).subscribe(products => {
      expect(products.length).toBe(2);
      expect(products).toEqual(mockProducts);
    });

    const req = httpMock.expectOne((request) => request.url === apiUrl && request.params.has('sort') && request.params.has('expansiveThan2PerLiter'));
    expect(req.request.method).toBe('GET');
    req.flush(mockProducts);
  });

  afterEach(() => {
    httpMock.verify();
  });
});

