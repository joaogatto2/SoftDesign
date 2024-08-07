import { BaseApi } from "./base-api";

export class BooksApi extends BaseApi {
    async getAll() {
        return this.get('/books');
    }   
    
    async getById(id: number) {
        return this.get(`/books/${id}`);
    }

    async rent(id: number) {
        return this.put(`/books/${id}/rent`, {});
    }
    
    async giveBack(id: number) {
        return this.put(`/books/${id}/give-back`, {});
    }
}