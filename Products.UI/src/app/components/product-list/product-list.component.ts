import { Component, OnInit } from '@angular/core';
import { ProductService } from '../../services/product.service';
import { IProduct } from '../../Dto/iproduct.dto';
import { CommonModule } from '@angular/common';
import { IArticle } from '../../Dto/iarticle.dto';

@Component({
  selector: 'app-product-list',
  imports: [CommonModule],
  templateUrl: './product-list.component.html',
  styleUrl: './product-list.component.scss'
})
export class ProductListComponent implements OnInit{
   articles:IArticle[]=[];
  viewMode: 'detail' | 'bottle' = 'detail';
  sortOrder: 'asc' | 'desc' = 'asc';
  filterExpensive: boolean = false;

  constructor(private productService: ProductService) {}

  ngOnInit(): void {
    this.loadProducts();
  }

  toggleView(): void {
    this.viewMode = this.viewMode === 'detail' ? 'bottle' : 'detail';
  }

  toggleSort(): void {
    this.sortOrder = this.sortOrder === 'asc' ? 'desc' : 'asc';
    this.loadProducts();
  }

  toggleFilter(): void {
    this.filterExpensive = !this.filterExpensive;
    this.loadProducts();
  }

  loadProducts(): void {
    this.productService
      .getProducts(this.sortOrder, this.filterExpensive)
      .subscribe((data) => (this.articles = data));
  }
}
