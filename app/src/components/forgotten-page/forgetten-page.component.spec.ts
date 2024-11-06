import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ForgettenPageComponent } from './forgetten-page.component';

describe('ForgettenPageComponent', () => {
  let component: ForgettenPageComponent;
  let fixture: ComponentFixture<ForgettenPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ForgettenPageComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ForgettenPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
