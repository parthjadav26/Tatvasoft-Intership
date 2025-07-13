import { DatePipe, NgFor, NgIf } from "@angular/common";
import { Component, OnDestroy, OnInit } from "@angular/core";
import { AbstractControl, FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from "@angular/forms";
import { ActivatedRoute, Router } from "@angular/router";
import { NgToastService } from "ng-angular-popup";
import { ToastrService } from "ngx-toastr";
import { CommonService } from "src/app/main/services/common.service";
import { MissionService } from "src/app/main/services/mission.service";
import { APP_CONFIG } from "src/app/main/configs/environment.config";
import { SidebarComponent } from "../../sidebar/sidebar.component";
import { HeaderComponent } from "../../header/header.component";
import { Subscription } from "rxjs";

// ✅ Custom validator: ensures startDate < endDate
function startDateBeforeEndDateValidator(group: AbstractControl): { [key: string]: any } | null {
  const startDate = group.get('startDate')?.value;
  const endDate = group.get('endDate')?.value;

  if (startDate && endDate && new Date(startDate) >= new Date(endDate)) {
    return { dateRangeInvalid: true };
  }
  return null;
}

@Component({
  selector: "app-update-mission",
  standalone: true,
  imports: [SidebarComponent, HeaderComponent, ReactiveFormsModule, NgIf, NgFor],
  templateUrl: "./update-mission.component.html",
  styleUrls: ["./update-mission.component.css"],
})
export class UpdateMissionComponent implements OnInit, OnDestroy {
  missionId: any;
  editData: any;
  editMissionForm: FormGroup;
  formValid: boolean = false;
  countryList: any[] = [];
  cityList: any[] = [];
  imageUrl: any[] = [];
  missionImage: any = "";
  isFileUpload = false;
  isDocUpload = false;
  missionDocName: any;
  missionDocText: any = "";
  formData = new FormData();
  formDoc = new FormData();
  missionThemeList: any[] = [];
  missionSkillList: any[] = [];
  typeFlag = false;
  imageListArray: any = [];
  existingImageNames: string[] = [];

  private unsubscribe: Subscription[] = [];

  constructor(
    private _fb: FormBuilder,
    private _service: MissionService,
    private _commonService: CommonService,
    private _toastr: ToastrService,
    private _router: Router,
    private _activateRoute: ActivatedRoute,
    private _datePipe: DatePipe,
    private _toast: NgToastService,
  ) {
    this.missionId = this._activateRoute.snapshot.paramMap.get("missionId");
    this.editMissionForm = this._fb.group({
      id: [""],
      missionTitle: ["", Validators.required],
      missionDescription: ["", Validators.required],
      countryId: ["", Validators.required],
      cityId: ["", Validators.required],
      startDate: ["", Validators.required],
      endDate: ["", Validators.required],
      totalSeats: [""],
      missionThemeId: ["", Validators.required],
      missionSkillId: ["", Validators.required],
      missionImages: [""],
    }, { validators: startDateBeforeEndDateValidator }); // ✅ Add validator
    if (this.missionId != 0) {
      this.fetchDetail(this.missionId);
    }
  }

  ngOnInit(): void {
    this.getCountryList();
    this.getMissionSkillList();
    this.getMissionThemeList();
  }

  getCountryList() {
    const subscription = this._commonService.countryList().subscribe((data: any) => {
      if (data.result == 1) this.countryList = data.data;
      else this._toast.error({ detail: "ERROR", summary: data.message, duration: APP_CONFIG.toastDuration });
    });
    this.unsubscribe.push(subscription);
  }

  getCityList(event: any) {
    const countryId = event.target.value;
    const subscription = this._commonService.cityList(countryId).subscribe((data: any) => {
      if (data.result == 1) this.cityList = data.data;
      else this._toast.error({ detail: "ERROR", summary: data.message, duration: APP_CONFIG.toastDuration });
    });
    this.unsubscribe.push(subscription);
  }

  getMissionSkillList() {
    const subscription = this._service.getMissionSkillList().subscribe(
      (data: any) => {
        if (data.result == 1) this.missionSkillList = data.data;
        else this._toast.error({ detail: "ERROR", summary: data.message, duration: APP_CONFIG.toastDuration });
      },
      (err) => this._toast.error({ detail: "ERROR", summary: err.message, duration: APP_CONFIG.toastDuration })
    );
    this.unsubscribe.push(subscription);
  }

  getMissionThemeList() {
    const subscription = this._service.getMissionThemeList().subscribe(
      (data: any) => {
        if (data.result == 1) this.missionThemeList = data.data;
        else this._toast.error({ detail: "ERROR", summary: data.message, duration: APP_CONFIG.toastDuration });
      },
      (err) => this._toast.error({ detail: "ERROR", summary: err.message, duration: APP_CONFIG.toastDuration })
    );
    this.unsubscribe.push(subscription);
  }

  fetchDetail(id: any) {
    const subscription = this._service.missionDetailById(id).subscribe((data: any) => {
      this.editData = data.data;
      this.editData.startDate = this._datePipe.transform(this.editData.startDate, "yyyy-MM-dd");
      this.editData.endDate = this._datePipe.transform(this.editData.endDate, "yyyy-MM-dd");

      this.editMissionForm.patchValue({
        id: this.editData.id,
        missionTitle: this.editData.missionTitle,
        missionDescription: this.editData.missionDescription,
        countryId: this.editData.countryId,
        cityId: this.editData.cityId,
        startDate: this.editData.startDate,
        endDate: this.editData.endDate,
        totalSeats: this.editData.totalSeats,
        missionThemeId: this.editData.missionThemeId,
        missionSkillId: this.editData.missionSkillId?.split(","),
        missionImages: "",
      });

      if (this.editData.missionImages) {
        const imageList = this.editData.missionImages;
        this.existingImageNames = imageList.split(",");
        this.imageUrl = [...this.existingImageNames];
        for (const photo of this.existingImageNames) {
          const cleaned = photo.replace(/\\/g, "/");
          this.imageListArray.push(this._service.imageUrl + "/" + cleaned);
        }
      }

      const cityListSubscription = this._commonService.cityList(this.editData.countryId).subscribe((data: any) => {
        this.cityList = data.data;
      });
      this.unsubscribe.push(cityListSubscription);
    });
    this.unsubscribe.push(subscription);
  }

  // Getters
  get countryId(): FormControl { return this.editMissionForm.get("countryId") as FormControl; }
  get cityId(): FormControl { return this.editMissionForm.get("cityId") as FormControl; }
  get missionTitle(): FormControl { return this.editMissionForm.get("missionTitle") as FormControl; }
  get missionDescription(): FormControl { return this.editMissionForm.get("missionDescription") as FormControl; }
  get startDate(): FormControl { return this.editMissionForm.get("startDate") as FormControl; }
  get endDate(): FormControl { return this.editMissionForm.get("endDate") as FormControl; }
  get missionThemeId(): FormControl { return this.editMissionForm.get("missionThemeId") as FormControl; }
  get missionSkillId(): FormControl { return this.editMissionForm.get("missionSkillId") as FormControl; }
  get missionImages(): FormControl { return this.editMissionForm.get("missionImages") as FormControl; }

  onSelectedImage(event: any) {
    const files = event.target.files;
    if (!files || files.length === 0) return;
    if ((this.imageListArray.length + files.length) > 6) {
      return this._toast.error({ detail: "ERROR", summary: "Maximum 6 images allowed", duration: APP_CONFIG.toastDuration });
    }
    this.isFileUpload = true;
    this.formData = new FormData();
    for (const file of files) {
      const reader = new FileReader();
      reader.onload = (e: any) => this.imageListArray.push(e.target.result);
      reader.readAsDataURL(file);
      this.formData.append("file", file);
      this.formData.append("moduleName", "Mission");
    }
  }

  async onSubmit() {
    this.formValid = true;
    if (!this.editMissionForm.valid) return;
    if (this.editMissionForm.errors?.['dateRangeInvalid']) return;

    const value = this.editMissionForm.value;
    value.missionSkillId = Array.isArray(value.missionSkillId) ? value.missionSkillId.join(",") : value.missionSkillId;

    let newImageNames: string[] = [];

    if (this.isFileUpload && this.formData.has("file")) {
      try {
        const res: any = await this._commonService.uploadImage(this.formData).toPromise();
        if (res.success) {
          newImageNames = res.data.map((img: string) => img.trim());
        }
      } catch (err: any) {
        this._toast.error({ detail: "ERROR", summary: err?.message || "Image upload failed" });
      }
    }

    const allImages = Array.from(new Set([...(this.existingImageNames || []), ...newImageNames]));
    value.missionImages = allImages.join(",");

    const subscription = this._service.updateMission(value).subscribe(
      (data: any) => {
        if (data.result === 1) {
          this._toast.success({ detail: "SUCCESS", summary: data.data, duration: APP_CONFIG.toastDuration });
          setTimeout(() => this._router.navigate(["admin/mission"]), 1000);
        } else {
          this._toastr.error(data.message);
        }
      },
      (err) => this._toast.error({ detail: "ERROR", summary: err.message, duration: APP_CONFIG.toastDuration })
    );

    this.unsubscribe.push(subscription);
  }

  onCancel() {
    this._router.navigate(["admin/mission"]);
  }

  onRemoveImage(item: any) {
    const index: number = this.imageListArray.indexOf(item);
    if (index !== -1) {
      this.imageListArray.splice(index, 1);
      const filename = item.split("/").pop();
      const fileIndex = this.existingImageNames?.indexOf(filename);
      if (fileIndex !== -1 && fileIndex !== undefined) {
        this.existingImageNames.splice(fileIndex, 1);
      }
    }
  }

  ngOnDestroy() {
    this.unsubscribe.forEach((sb) => sb.unsubscribe());
  }
}
