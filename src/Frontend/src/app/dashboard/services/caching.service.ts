import { Injectable } from "@angular/core";
import { environment } from "src/environments/environment.development";

@Injectable({
    providedIn: 'root',
})
export class CachingService{
    private cachedData: { [key: string]: any } = new Map();
    private readonly cacheLength: number = environment.cacheStorageLength;

    set(key: string, value: any){
        let keys = Object.keys(this.cachedData);
        if(keys.length > this.cacheLength){
            let firstKey = keys.shift();
            delete this.cachedData[firstKey!];
        }

        this.cachedData[key] = value;
    }

    get(key: string): any{
        let keys = Object.keys(this.cachedData);
        if(keys.includes(key))
            return this.cachedData[key];

        return null;
    }

    remove(key: string): void{
        let keys = Object.keys(this.cachedData);
        if(keys.includes(key)){
            delete this.cachedData[key];
        }
    }
}
