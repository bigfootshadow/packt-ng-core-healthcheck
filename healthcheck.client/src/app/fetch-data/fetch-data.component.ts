import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { environment } from "../../environments/environment";

@Component({
  selector: 'app-fetch-data',
  templateUrl: './fetch-data.component.html',
  styleUrl: './fetch-data.component.css'
})
export class FetchDataComponent {

  public forecasts: WeatherForecast[] = [];

  constructor(private http: HttpClient) {
    this.getForecasts();
  }

  getForecasts() {
    this.http.get<WeatherForecast[]>(environment.baseUrl + 'api/WeatherForecast').subscribe(
      (result) => {
        this.forecasts = result;
      },
      (error) => {
        console.error(error);
      }
    );
  }

}

interface WeatherForecast {
  date: string;
  temperatureC: number;
  temperatureF: number;
  summary: string;
}
