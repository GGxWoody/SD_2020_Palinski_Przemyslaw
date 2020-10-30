/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { InviteListComponent } from './invite-list.component';

describe('InviteListComponent', () => {
  let component: InviteListComponent;
  let fixture: ComponentFixture<InviteListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ InviteListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(InviteListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
