import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddAcquisitionComponent } from './add-acquisition.component';

describe('AddAcquisitionComponent', () => {
  let component: AddAcquisitionComponent;
  let fixture: ComponentFixture<AddAcquisitionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AddAcquisitionComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AddAcquisitionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
