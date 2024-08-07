import axios from 'axios';

export class BaseApi {
    protected baseUrl = 'https://localhost:7174/api';

    protected post(url: string, data: unknown) {
        const token = localStorage.getItem('token');

        return axios.post(this.baseUrl + url, data, {
            headers: {
                Authorization: `Bearer ${token}`
            }
        });
    }

    protected get(url: string) {
        const token = localStorage.getItem('token');

        return axios.get(this.baseUrl + url, {
            headers: {
                Authorization: `Bearer ${token}`
            }
        });
    }

    protected put(url: string, data: unknown) {
        const token = localStorage.getItem('token');

        return axios.put(this.baseUrl + url, data, {
            headers: {
                Authorization: `Bearer ${token}`
            }
        });
    }
}