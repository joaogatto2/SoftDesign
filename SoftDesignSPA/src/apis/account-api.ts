import { BaseApi } from "./base-api";

export class AccountApi extends BaseApi {
    async login(email: string, password: string) {
        return this.post('/account/login', { email, password });
    }   
}