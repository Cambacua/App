
export interface CityDto {
  name?: string;
  country?: string;
}

export interface CitySearchRequestDto {
  partialName?: string;
  pais?: string;
}

export interface CitySearchResultDto {
  cities: CityDto[];
}
