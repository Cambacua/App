import { City } from './city';

describe('City model', () => {
  let model: City;

  beforeEach(() => {
    model = new City();
  });

  it('should create an instance', () => {
    expect(model).toBeTruthy();
  });
});
