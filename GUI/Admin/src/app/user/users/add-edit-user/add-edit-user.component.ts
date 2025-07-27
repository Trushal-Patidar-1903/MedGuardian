import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { User } from '../../../Services/UserService/user.service';

@Component({
  selector: 'app-add-edit-user',
  standalone: false,
  templateUrl: './add-edit-user.component.html',
  styleUrl: './add-edit-user.component.scss'
})
export class AddEditUserComponent {
  userForm!: FormGroup;
  isEditMode = false;
  userId: string | null = null;

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router
  ) {

    this.userForm = this.fb.group({
      name: ['', [Validators.required, Validators.minLength(2)]],
      email: ['', [Validators.required, Validators.email]]
    });
  }

  ngOnInit(): void {


    this.route.data.subscribe((data) => {
      console.log('Resolved user:', data['user']);
      // Now you can patch the form, etc.
    });

    this.userId = this.route.snapshot.paramMap.get('id');
    const userData = this.route.snapshot.data['user'] as User;

    this.isEditMode = !!this.userId;



    if (this.isEditMode && userData) {
      this.userForm.patchValue(userData);
    }
  }

  save(): void {
    if (this.userForm.invalid) return;

    const formData = this.userForm.value;

    if (this.isEditMode) {
      console.log('Update:', formData);
    } else {
      console.log('Create:', formData);
    }

    this.router.navigate(['/']);
  }

  cancel(): void {
    this.router.navigate(['/']);
  }

  get f() {
    return this.userForm.controls;
  }

}
