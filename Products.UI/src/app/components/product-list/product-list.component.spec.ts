import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ProductListComponent } from './product-list.component';  // No need to declare here
import { ProductService } from '../../services/product.service';
import { HttpClientTestingModule } from '@angular/common/http/testing'; 
import { of } from 'rxjs';
import { IArticle } from '../../Dto/iarticle.dto';
import { CommonModule } from '@angular/common';

describe('ProductListComponent', () => {
  let component: ProductListComponent;
  let fixture: ComponentFixture<ProductListComponent>;
  let mockProductService: jasmine.SpyObj<ProductService>;
  const mockArticles: IArticle[] = [
    {
      id: 1,
      productName: 'Test Beer',
      price: 1.99,
      shortDescription: 'A test beer',
      pricePerUnitText: '1.99 €/L',
      image: 'test.jpg',
      unit: '1L'
    }
  ];

  beforeEach(async () => {
    mockProductService = jasmine.createSpyObj('ProductService', ['getProducts']);
    mockProductService.getProducts.and.returnValue(of(mockArticles));

    await TestBed.configureTestingModule({
      imports: [CommonModule, HttpClientTestingModule, ProductListComponent],  
      providers: [{ provide: ProductService, useValue: mockProductService }]
    }).compileComponents();

    fixture = TestBed.createComponent(ProductListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should load products on init', () => {
    expect(mockProductService.getProducts).toHaveBeenCalledWith('asc', false);
    expect(component.articles.length).toBe(1);
  });

  it('should toggle view mode', () => {
    expect(component.viewMode).toBe('detail');
    component.toggleView();
    expect(component.viewMode).toBe('bottle');
    component.toggleView();
    expect(component.viewMode).toBe('detail');
  });

  it('should toggle sort order and reload products', () => {
    component.sortOrder = 'asc';
    component.toggleSort();
    expect(component.sortOrder).toBe('desc');
    expect(mockProductService.getProducts).toHaveBeenCalledWith('desc', false);
  });

  it('should toggle filter and reload products', () => {
    component.filterExpensive = false;
    component.toggleFilter();
    expect(component.filterExpensive).toBeTrue();
    expect(mockProductService.getProducts).toHaveBeenCalledWith('asc', true);
  });
});
