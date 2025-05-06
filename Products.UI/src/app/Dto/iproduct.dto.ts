import { IArticle } from "./iarticle.dto";

export interface IProduct {
    id: number;
    brandName: string;
    name: string;
    descriptionText?: string;
    articles: IArticle[];
  }