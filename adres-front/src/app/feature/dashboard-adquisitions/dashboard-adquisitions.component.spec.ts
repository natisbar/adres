import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DashboardAdquisitionsComponent } from './dashboard-adquisitions.component';

describe('DashboardAdquisitionsComponent', () => {
  let component: DashboardAdquisitionsComponent;
  let fixture: ComponentFixture<DashboardAdquisitionsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [DashboardAdquisitionsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DashboardAdquisitionsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
