import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StudentdetailsPageComponent } from './studentdetails-page.component';

describe('StudentdetailsPageComponent', () => {
  let component: StudentdetailsPageComponent;
  let fixture: ComponentFixture<StudentdetailsPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [StudentdetailsPageComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(StudentdetailsPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
