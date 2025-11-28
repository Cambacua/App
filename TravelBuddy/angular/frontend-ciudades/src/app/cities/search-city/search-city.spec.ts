import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SearchCityComponent } from './search-city';

describe('SearchCity', () => {
  let component: SearchCityComponent;
  let fixture: ComponentFixture<SearchCityComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SearchCityComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SearchCityComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
