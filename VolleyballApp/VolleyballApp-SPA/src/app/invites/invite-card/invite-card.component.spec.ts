/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { InviteCardComponent } from './invite-card.component';

describe('InviteCardComponent', () => {
  let component: InviteCardComponent;
  let fixture: ComponentFixture<InviteCardComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ InviteCardComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(InviteCardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
